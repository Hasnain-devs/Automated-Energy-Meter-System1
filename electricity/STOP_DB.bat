@echo off
set BASE=C:\Users\Hp\mariadb\mariadb-12.2.2-winx64
"%BASE%\bin\mysqladmin.exe" --protocol=tcp --ssl=0 -h 127.0.0.1 -P 3306 -u root -pRoot@123 shutdown
if %errorlevel%==0 (
  echo MariaDB stopped.
) else (
  echo Could not stop MariaDB (it may already be stopped).
)
