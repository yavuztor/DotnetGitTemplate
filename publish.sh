#! /bin/bash
rm bin/Debug/*.nupkg
dotnet pack
PKGNAME=`ls bin/Debug/*.nupkg`
echo PKGNAME is $PKGNAME
dotnet nuget push $PKGNAME -k "$NUGET_KEY" -s "https://api.nuget.org/v3/index.json"