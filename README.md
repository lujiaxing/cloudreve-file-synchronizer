# Cloudreve File Synchronizer

A simple post-offline-copy file list synchronizer.

- This tool is used to post-record data for files that not uploaded by Cloudreve but uploaded via ftp or disk copy.
- This tool won't perform a real file copy but just make files under control in Cloudreve.
- Only work for Cloudreve version 3.0+ (using MySQL database)



Usage:

```
Cloudreve_FileSynchronizer -s /source/directory -t /target/path/in/cloudreve -n user-name -p policy-id -c "Server=xxx;Port=3306;database=Cloudreve_Database;uid=xxxx;pwd=xxx;"
```

Options:

  --s, --source-dir=VALUE     source directory to upload

  --n, --user-name=VALUE      upload for whom

  --p, --policy-id=VALUE      storage policy id

  --t, --target=VALUE         base directory path

  --c, --connection-string=VALUE Connection string to a mysql instance

  --h, --help                 show help
