using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbScheme
    {
        // ---------------------------- Phần khai báo thuộc tính ----------------------------

        // Tên lược đồ
        public string schemename { get; set; }

        // Danh sách các thuộc tính
        public List<ProbAttribute> attributes { get; set; }



        // ---------------------------- Phần định nghĩa các phương thức ----------------------------

        public ProbScheme()
        {
            this.attributes = new List<ProbAttribute>();
        }

        public ProbScheme(string schemename)
        {
            this.schemename = schemename;
            this.attributes = new List<ProbAttribute>();
        }

        public bool isInherited(List<ProbRelation> Relations)
        {
            try
            {
                if (this.attributes.Count > 0)
                {
                    foreach (ProbRelation relation in Relations)
                        if (this.Equals(relation.scheme))
                            if (relation.tuples.Count > 0)
                                return true;
                }
            }
            catch { }
            return false;
        }

        public ProbAttribute GetAttribute(string attrName)
        {
            try
            {
                foreach (ProbAttribute attribute in this.attributes)
                    if (attribute.attributeName.Equals(attrName))
                        return attribute;
                return null;
            }
            catch (Exception Ex)
            {
                return null;
            }            
        }

        public bool ContainAttribute(ProbAttribute attribute)
        {
            try
            {
                foreach (ProbAttribute attr in this.attributes)
                    if (attr.attributeName.Equals(attribute.attributeName))
                        return true;
                return false;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public void Assign(ProbScheme scheme)
        {
            try
            {
                if (scheme == null) return;
                this.schemename = scheme.schemename;
                this.attributes = new List<ProbAttribute>();
                ProbAttribute thisAttr;
                foreach (ProbAttribute attr in scheme.attributes)
                {
                    thisAttr = new ProbAttribute();
                    thisAttr.Assign(attr);
                    this.attributes.Add(thisAttr);
                }
            }
            catch (Exception Ex) { }
        }
    }
}
