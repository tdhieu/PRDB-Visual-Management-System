using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRDB_Visual_Management.PresentationLayer;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbRelation
    {
        // ---------------------------- Phần khai báo thuộc tính ----------------------------

        // Lược đồ quan hệ tương ứng
        public ProbScheme scheme { get; set; }

        // Tên quan hệ
        public string relationname { get; set; }

        // Tập các bộ trên quan hệ
        public List<ProbTuple> tuples { get; set; }



        // ---------------------------- Phần định nghĩa các phương thức ----------------------------

        public ProbRelation() // Constructor
        {
            this.scheme = new ProbScheme();
            this.relationname = string.Empty;
            this.tuples = new List<ProbTuple>();
        }

        public ProbRelation(string relname) // Constructor
        {
            this.scheme = new ProbScheme();
            this.tuples = new List<ProbTuple>();
            this.relationname = relname;
        }

        public string CutHeading(string S, string P)
        {
            for (int i = 0; i < S.Length; i++)
                if (S.Substring(0, i + 1).Equals(P))
                {
                    S = S.Remove(0, i + 1);
                    break;
                }
            return S;
        }

        public void AssignValue(ProbRelation relation)
        {
            try
            {
                if (relation == null) return;
                this.scheme.Assign(relation.scheme);
                this.relationname = relation.relationname;
                ProbTuple newTuple;
                foreach (ProbTuple tuple in relation.tuples)
                {
                    newTuple = new ProbTuple();
                    newTuple.Assign(tuple);
                    this.tuples.Add(newTuple);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public ProbTuple GetTupleSamePrimaryKey(ProbTuple tuple)
            // Lấy ra bộ tương ứng trong quan hệ có cùng khóa với bộ đang xét
        {
            try
            {
                // Tồn tại 1 bộ trong quan hệ có cùng tập khóa với bộ đang xét
                foreach (ProbTuple thisTuple in this.tuples)
                    if (thisTuple.SamePrimaryKeyWith(tuple))
                        return thisTuple;
                return null;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public bool Contain(ProbAttribute attribute)
        {
            try
            {
                foreach (ProbAttribute attr in this.scheme.attributes)
                    if (attr.IsEqualTo(attribute))
                        return true;
                return false;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public void GetTuplesOf(ProbRelation srcRelation)
        {
            try
            {
                if (srcRelation == null) return;
                foreach (ProbTuple srctuple in srcRelation.tuples)
                    this.tuples.Add(srctuple);
            }
            catch (Exception Ex)
            {
            }
        }

        public void GetAttributesOf(ProbRelation srcRelation)
        {
            try
            {
                if (srcRelation == null) return;
                foreach (ProbAttribute attr in srcRelation.scheme.attributes)
                    this.scheme.attributes.Add(attr);
            }
            catch (Exception Ex)
            {
            }
        }

        public void Remove(ProbAttribute probattr)
        {
            try
            {
                foreach (ProbAttribute attr in this.scheme.attributes)
                    if (attr.IsEqualTo(probattr))
                    {
                        this.scheme.attributes.Remove(attr);
                        foreach (ProbTuple tuple in this.tuples)
                            tuple.triples.Remove(attr);
                        break;
                    }
            }
            catch (Exception Ex) { }
        }
    }
}
