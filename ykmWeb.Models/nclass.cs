
using System.ComponentModel.DataAnnotations;

namespace ykmWeb.Models
{
  
    public class nclass
    {
  

        [Key]
        public int? Catalogid
        {
            get;set;
        }

        public string Catalogname
        {
            get;set;
        }
    

        public int? ParentID
        {
            get;set;
        }
     

        public string ParentStr
        {
            get;set;
        }
   

        public int? Depth
        {
            get;set;
        }
     

        public int? RootID
        {
            get;set;
        }
     

        public int? Orders
        {
            get;set;
        }
  

        public int? Child
        {
            get;set;
        }

 
        public string Caenname
        {
            get;set;
        }

    }
   
}
