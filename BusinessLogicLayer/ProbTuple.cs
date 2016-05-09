using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbTuple    // Biến bộ giá trị xác suất
    {
        // ---------------------------- Phần khai báo thuộc tính ----------------------------

        // Tập các giá trị bộ ba xác suất trên một tuple
        public Dictionary<ProbAttribute, ProbTriple> triples { get; set; }


        // ---------------------------- Phần định nghĩa các phương thức ----------------------------

        public ProbTuple()
        {
            this.triples = new Dictionary<ProbAttribute, ProbTriple>();
        }

        public ProbAttribute GetAttribute(string name)
        {
            try
            {
                foreach (ProbAttribute attribute in this.triples.Keys)
                    if (attribute.attributeName.Equals(name))
                        return attribute;
                return null;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public bool IsValid()
        {
            try
            {
                foreach (ProbTriple triple in this.triples.Values)
                    // Nếu trong bộ tồn tại một bộ ba có xác suất không có giá trị => bộ không hợp lệ
                    if (triple.values.Count <= 0)
                        return false;
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public void Assign(ProbTuple tuple)
        {
            try
            {
                if (tuple == null) return;
                KeyValuePair<ProbAttribute, ProbTriple> thisTriple;
                foreach (KeyValuePair<ProbAttribute, ProbTriple> triple in tuple.triples)
                {
                    thisTriple = new KeyValuePair<ProbAttribute, ProbTriple>();
                    thisTriple = triple;
                    this.triples.Add(thisTriple.Key, thisTriple.Value);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public bool SamePrimaryKeyWith(ProbTuple checkTuple)
        {
            try
            {
                List<ProbAttribute> primaryKeys = new List<ProbAttribute>();

                // Lấy danh sách các thuộc tính là PrimaryKey trên bộ hiện tại
                foreach (ProbAttribute attr in this.triples.Keys)
                    if (attr.IsPrimaryKey())
                        primaryKeys.Add(attr);

                bool checkEqual;

                // Kiểm tra các PrimaryKeys trên bộ hiện tại có bằng với các PrimaryKeys trên checkTuple
                foreach (ProbAttribute checkAttribute in checkTuple.triples.Keys)
                    if (checkAttribute.IsPrimaryKey())
                    {
                        checkEqual = false;
                        foreach (ProbAttribute pkAttribute in primaryKeys)
                            if (checkAttribute.IsEqualTo(pkAttribute))
                                if (checkTuple.triples[checkAttribute].IsEqualTo(this.triples[pkAttribute], pkAttribute.type.typeName))
                                {
                                    checkEqual = true;
                                    break;
                                }
                        if (checkEqual == false) return false;
                    }
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public ProbTriple GetTriple(ProbAttribute attribute)
        {
            try
            {
                foreach (ProbAttribute thisAttribute in this.triples.Keys)
                    if (thisAttribute.attributeName.Equals(attribute.attributeName))
                        return this.triples[thisAttribute];                
            }
            catch (Exception Ex)
            {
            }
            return null;
        }
    }
}
