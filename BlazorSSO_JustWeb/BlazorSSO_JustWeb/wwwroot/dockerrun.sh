#!/bin/sh

local_var=$(printenv "GOOGLE_CLIENTID")
echo "Google client id is: $local_var"

sed -i "s|GOOGLE_CLIENT_ID_PLACEHOLDER|${local_var}|g" appsettings.json
echo "replaced value 'GoogleSSO:ClientId' in appsettings.json with ${local_var}"