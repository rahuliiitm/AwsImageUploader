using System;
using System.Collections.Generic;
using System.Text;

namespace ImageUploader.Models
{

    public class ConfirmModel
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public ImageUploaderStatus Status { get; set; }
    }
}
