using Cloudreve_FileSynchronizer.EF;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudreve_FileSynchronizer
{
    class Program
    {
        private const string IMAGE_EXT = ".jpg|.jpeg|.gif|.tiff|.png|.svg";

        static void Main(string[] args)
        {
            ProgramParameter parameter = ParseParameter(args);

            if (parameter == null)
                return;

            if(!Directory.Exists(parameter.SourceDirectory))
            {
                Console.WriteLine("Error: source directory does not exist");
                return;
            }

            DirectoryInfo srcDir = new DirectoryInfo(parameter.SourceDirectory);

            Task.WaitAll(Task.Run(async () =>
            {
                using (var dbContext = new CloudreveContext(parameter.ConnectionString))
                {
                    var user = dbContext.Users.FirstOrDefault(p =>
                        p.Email == parameter.UserName || p.Nick == parameter.UserName);

                    if (user == null)
                    {
                        Console.WriteLine($"Error: user \"{parameter.UserName}\" does not exist");
                        return;
                    }

                    var policy = dbContext.Policies.FirstOrDefault(p => p.Id == parameter.StoragePolicy);

                    if (policy == null)
                    {
                        Console.WriteLine($"Error: Storage policy (ID = \"{parameter.StoragePolicy}\") does not exist");
                        return;
                    }

                    Folders rootFolder = FindRootFolder(parameter.Target, dbContext);

                    if (rootFolder == null)
                    {
                        Console.WriteLine($"target path \"{parameter.Target}\" does not exist in Cloudreve's database");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"target path \"{parameter.Target}\" found. ID = {rootFolder.Id}");
                        Console.WriteLine($"Synchronizing files and directories");
                    }

                    var trans = await dbContext.Database.BeginTransactionAsync();

                    try
                    {
                        await SaveTree(srcDir, rootFolder, parameter.Target, user, policy, dbContext);

                        await dbContext.SaveChangesAsync();

                        await trans.CommitAsync();

                        Console.WriteLine($"Transaction committed.");
                        Console.WriteLine($"Successed.");
                    }
                    catch(Exception ex)
                    {
                        await trans.RollbackAsync();

                        Console.WriteLine($"An has error occured durning synchronization: {ex.Message}{(ex.InnerException == null ? "" : $" ({ex.InnerException.Message})")}");
                        Console.WriteLine("Stack trace:");
                        Console.WriteLine(ex.StackTrace);
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                    
                }
            }));

        }


        public static async Task SaveTree(DirectoryInfo dir, Folders parentDir, 
            string thisDirPath, Users user, Policies policy, CloudreveContext dbContext)
        {
            string dirName = dir.Name;

            Console.WriteLine($" - Processing directory \"{thisDirPath}\"");

            var newFolder = dbContext.Folders.Add(new Folders()
            {
                OwnerId = user.Id,
                Name = dirName,
                ParentId = parentDir.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            await dbContext.SaveChangesAsync();

            Console.WriteLine($" - Folder \"{thisDirPath}\" created. ID = {newFolder.Entity.Id}");

            var files = 
            dir.GetFiles().Select(p =>
            {
                string picinfo = null, fileFullPath;

                if (!String.IsNullOrEmpty(p.Extension) && IMAGE_EXT.Contains(p.Extension.ToLower()))
                    picinfo = GetImageSizeStr(p);

                fileFullPath = (thisDirPath + "/" + p.Name);

                var file = new Files()
                {
                    FolderId = newFolder.Entity.Id,
                    Name = p.Name,
                    PicInfo = picinfo,
                    PolicyId = policy.Id,
                    Size = (ulong)p.Length,
                    SourceName = fileFullPath,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                Console.WriteLine($" - File \"{fileFullPath}\" created.");

                return file;
            });

            await dbContext.Files.AddRangeAsync(files);

            Console.WriteLine($" - {files.Count()} files created.");

            var subDirectories = dir.GetDirectories();
            if (subDirectories.Length > 0)
            {
                Console.WriteLine($" - {subDirectories.Length} sub directories found. Processing sub directories...");

                foreach (var subDir in subDirectories)
                    await SaveTree(subDir, newFolder.Entity, (thisDirPath + "/" + subDir.Name), user, policy, dbContext);
            }
            else
            {
                Console.WriteLine($" - Leaf directory, leaving...");
            }

        }

        private static string GetImageSizeStr(FileInfo p)
        {
            string picinfo = null;
            FileStream imageFileStream = null;
            Image image = null;
            try
            {
                imageFileStream = p.Open(FileMode.Open);

                image = Image.FromStream(imageFileStream, false, false);

                picinfo = $"{image.Width},{image.Height}";

            }
            catch(Exception ex)
            {
                Console.WriteLine($" - \"{p.Name}\" Failed to get image dimensions: {ex.Message}");
            }
            finally
            {
                if (imageFileStream != null)
                   imageFileStream.Dispose();

                if (image != null)
                    image.Dispose();
            }

            return picinfo;
        }

        private static Folders FindRootFolder(string targetPath, CloudreveContext dbContext)
        {
            Folders rootFolder = null;

            if (targetPath == "/")
            {
                rootFolder = dbContext.Folders.FirstOrDefault(p => p.Name == "/");
            }
            else
            {

                var pathParts = targetPath.Split('/');

                if (String.IsNullOrWhiteSpace(pathParts.Last()))
                    pathParts = pathParts.Take(pathParts.Length - 1).ToArray();

                var beginning = pathParts.Last();

                var folderRecord = dbContext.Folders.Where(p => p.Name == beginning).ToArray();

                foreach (var rec in folderRecord)
                {
                    int i = pathParts.Length - 1;
                    var cur = rec;

                    do i--;while (i > 0 && (cur = dbContext.Folders.FirstOrDefault(p => p.Id == cur.ParentId))?.Name == pathParts[i]);

                    if (i == 0)
                    {
                        rootFolder = rec;
                        break;
                    }
                }
            }
            
            return rootFolder;
        }

        private static ProgramParameter ParseParameter(string[] args)
        {
            ProgramParameter programParameter = new ProgramParameter();
            bool success = true;

            //string connectionString = "Server={SERVER};Port={PORT};database={DATABASE};uid={UID};pwd={PWD};SslMode=None";
            var opt = new OptionSet() {
                {
                    "s|source-dir=", "source directory to upload",
                    v => {

                        if(String.IsNullOrEmpty(v))
                        {
                            Console.WriteLine("source directory cannot be empty");
                            success = false;
                        }
                        else
                            programParameter.SourceDirectory = v;
                    }
                },
                {
                    "n|user-name=", "upload for whom",
                    v =>
                    {
                        if(String.IsNullOrEmpty(v))
                        {
                            Console.WriteLine("user name cannot be empty");
                            success = false;
                        }
                        else
                            programParameter.UserName = v;
                    }
                },
                {
                    "p|policy-id=", "storage policy id",
                    v =>
                    {
                        if(String.IsNullOrEmpty(v))
                        {
                            Console.WriteLine("storage policy id cannot be empty");
                            success = false;
                        }

                        programParameter.StoragePolicy = Int32.Parse(v);
                    }
                },
                {
                    "t|target=", "base directory path",
                    v =>
                    {
                        if(String.IsNullOrEmpty(v))
                        {
                            Console.WriteLine("target directory cannot be empty");
                            success = false;
                        }
                        else if (!v.StartsWith("/"))
                        {
                            Console.WriteLine("target must start with \"/\"");
                            success = false;
                        }
                            
                        else
                            programParameter.Target = v;
                    }
                },
                {
                    "c|connection-string=", "Connection string to a mysql instance",
                    v =>
                    {
                        if(String.IsNullOrEmpty(v))
                        {
                            Console.WriteLine("connection string cannot be empty");
                            success = false;
                        }
                        else
                        {
                            programParameter.ConnectionString = v;
                        }


                    }
                },
                {
                    "h|help", "show help",
                    v =>
                    {
                        //do nothing
                    }
                }
            };

            if (args == null || args.Length == 0 || args.Any(p => p == "-h" || p == "--help"))
            {
                ShowHelp(opt);
                return null;
            }

            List<string> extra;
            try
            {
                extra = opt.Parse(args);

                if (!success)
                    return null;

                return programParameter;
            }
            catch (OptionException e)
            {
                Console.Write("invalid parameter: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `--help' for more information.");
                return null;
            }
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("This tool is used to post-record data for files that not uploaded by Cloudreve but uploaded via ftp or disk copy.");
            Console.WriteLine("This won't perform a real file copy but just make files under control in Cloudreve.");
            Console.WriteLine("this tool only work for Cloudreve version 3.0+ (using MySQL database)");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
