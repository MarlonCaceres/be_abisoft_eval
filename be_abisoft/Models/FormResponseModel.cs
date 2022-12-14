using System;
namespace be_abisoft.Models
{
    public class FormResponseModel
    {
        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }

        public FormResponseModel()
        {
            this.codError = "-1";
            this.msgError = "Error de comunicación";
            this.success = false;
            root = new List<Object>();

        }
    }
}

