using System;
using System.Collections.Generic;
using System.Text;

namespace Go2Share.Entity.Entity
{
    public class DeleteModel
    {
        public DeleteModel() { }

        public DeleteModel(string HeaderText, string BodyText, string ModelId, string APIUrl,string TableId,string ModalId)
        {
            this.HeaderText = HeaderText;
            this.BodyText = BodyText;
            this.ModelId = ModelId;
            this.APIUrl = APIUrl;
            this.TableId = TableId;
            this.ModalId = ModalId;
        }

        public string HeaderText { get; set; }
        public string BodyText { get; set; }
        public string ModelId { get; set; }
        public string APIUrl { get; set; }
        public string TableId { get; set; }
        public string ModalId { get; set; }
    }
}
