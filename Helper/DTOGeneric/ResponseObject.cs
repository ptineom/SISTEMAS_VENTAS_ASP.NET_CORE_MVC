using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Helper.DTOGeneric
{
    public class ResponseObject 
    {
        public object Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public ErrorDetails ErrorDetails { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
