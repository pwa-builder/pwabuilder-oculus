﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.PWABuilder.Oculus.Models
{
    /// <summary>
    /// Exception that contains information about a process run, such as standard output and standard error.
    /// </summary>
    [Serializable]
    public class ProcessException : Exception
    {
        public ProcessException() { }
        public ProcessException(string message, string? standardOutput, string? standardError) : base(message) 
        {
            this.StandardOutput = standardOutput;
            this.StandardError = standardError;
        }
        public ProcessException(string message, Exception inner, string? standardOutput, string? standardError) : base(message, inner) 
        {
            this.StandardOutput = standardOutput;
            this.StandardError = standardError;
        }
        protected ProcessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public string? StandardOutput { get;set; }
        public string? StandardError { get; set; }
    }
}
