#!/usr/bin/env sh
nupkgs=(Lightwind.AsyncInterceptor Lightwind.DynamicProxyExtension)

for nupkg in ${nupkgs[@]}
do
  cd $nupkg
  rm -rf bin/Release/*.nupkg
  dotnet pack -c Release
  dotnet nuget push bin/Release/*.nupkg -k $NUGET_APIKEY_LIGHTWIND -s https://api.nuget.org/v3/index.json
  cd ..
done
echo "nuget publish finish"
