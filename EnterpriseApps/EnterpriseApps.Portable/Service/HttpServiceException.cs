﻿using System;

namespace EnterpriseApps.Portable.Service
{
    public  class HttpServiceException : Exception
    {
        public HttpServiceException()
        {
        }

        public HttpServiceException(string message) : base(message)
        {
        }

        public HttpServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}