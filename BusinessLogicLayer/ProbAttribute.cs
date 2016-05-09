using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbAttribute
    {
        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        public bool primaryKey { get; set; }

        public string attributeName { get; set; }

        public ProbDataType type { get; set; }

        public string description { get; set; }

        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        public ProbAttribute()
        {
            this.type = new ProbDataType();
        }

        public void Assign(ProbAttribute attribute)
        {
            try
            {
                if (attribute == null) return;
                this.primaryKey = attribute.primaryKey;
                this.attributeName = attribute.attributeName;
                this.type.Assign(attribute.type);
                this.description = attribute.description;
            }
            catch (Exception Ex)
            {
            }
        }

        public bool IsPrimaryKey()
        {
            return this.primaryKey;
        }

        public bool IsEqualTo(ProbAttribute attribute)
        {
            try
            {
                if (this.primaryKey != attribute.primaryKey) return false;
                if (!this.attributeName.Equals(attribute.attributeName)) return false;
                if (!this.type.IsEqualTo(attribute.type)) return false;
                if (!this.description.Equals(attribute.description)) return false;
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public bool NotInList(List<ProbAttribute> listAttr)
        {
            try
            {
                if (listAttr == null) return true;
                foreach (ProbAttribute srcAttr in listAttr)
                    if (this.IsEqualTo(srcAttr))
                        return false;
                return true;
            }
            catch (Exception Ex) { return true;  }
        }
    }
}
