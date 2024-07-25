# FtpDownloader
This is a simple FTP downloader that downloads files from a FTP server. It is written in C# and uses the FluentFtp library.

## Usage
create a JSON file with the following structure(ftp_config.json):

```json
  [
    {
      "Host" : "host1.com",
      "Username": "username",
      "Password": "password",
      "RemoteDirectory": "/remoteFolder/*.txt",
      "LocalDirectory": "C:\\localFolder"
    },
    {
      "Host" : "host2.com",
      "Username": "username",
      "Password": "password",
      "RemoteDirectory": "/remoteFolder/*",
      "LocalDirectory": "C:\\localfolder"
    }
]
```