#!/bin/sh

dotnet publish

RLS=$(find . -wholename "./bin/Debug/*.nupkg" -print0 | xargs -r -0 ls -1 -t | head -1)

echo "$RLS"

dotnet nuget push "$RLS" --source "https://api.nuget.org/v3/index.json" --api-key="$SEVEN_NUGET_API_KEY"