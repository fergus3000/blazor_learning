﻿namespace BlazorSSO.WebApi
{
    public class JwtTokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
