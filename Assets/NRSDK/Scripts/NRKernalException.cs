using System;

namespace NRKernal
{
    public class NRKernalError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRKernalError()
        {

        }
        public NRKernalError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRKernalError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }

    public class NRInvalidArgumentError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRInvalidArgumentError()
        {

        }
        public NRInvalidArgumentError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRInvalidArgumentError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }

    public class NRNotEnoughMemoryError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRNotEnoughMemoryError()
        {

        }
        public NRNotEnoughMemoryError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRNotEnoughMemoryError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }

    public class NRSdcardPermissionDenyError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRSdcardPermissionDenyError()
        {

        }
        public NRSdcardPermissionDenyError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRSdcardPermissionDenyError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }

    public class NRUnSupportedError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRUnSupportedError()
        {

        }
        public NRUnSupportedError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRUnSupportedError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }

    public class NRGlassesConnectError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRGlassesConnectError()
        {

        }
        public NRGlassesConnectError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRGlassesConnectError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }

    public class NRSdkVersionMismatchError : ApplicationException
    {
        private string error;
        private Exception innerException;
        public NRSdkVersionMismatchError()
        {

        }
        public NRSdkVersionMismatchError(string msg) : base(msg)
        {
            this.error = msg;
        }
        public NRSdkVersionMismatchError(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.error = msg;
        }
        public string GetError()
        {
            return error;
        }
    }
}
