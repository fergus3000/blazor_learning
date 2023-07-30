 #!/bin/sh
google_oidp_clientid=$(printenv "GOOGLE_CLIENTID")
echo "Google client id is: ${google_oidp_clientid}"
sed -i "s|GOOGLE_CLIENT_ID_PLACEHOLDER|${google_oidp_clientid}|g" /usr/share/nginx/html/appsettings.json
echo "replaced value 'GoogleSSO:ClientId' in appsettings.json with ${google_oidp_clientid}"