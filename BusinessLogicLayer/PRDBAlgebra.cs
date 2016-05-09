using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PRDB_Visual_Management.ProbSQLCompiler;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public static class PRDBAlgebra
    {
        public static ProbRelation Combine(ProbRelation r1, ProbRelation r2, string connectorType)
            // Phương thức gọi hàm để xử lý phép toán kết hợp giữa 2 quan hệ
        {
            try
            {
                switch (connectorType)
                {
                    case "UNION_IN": return UNION_IN(r1, r2);
                    case "UNION_IG": return UNION_IG(r1, r2);
                    case "UNION_ME": return UNION_ME(r1, r2);
                    case "UNION_PC": return UNION_PC(r1, r2);
                    case "INTERSECT_IN": return INTERSECT_IN(r1, r2);
                    case "INTERSECT_IG": return INTERSECT_IG(r1, r2);
                    case "INTERSECT_ME": return INTERSECT_ME(r1, r2);
                    case "INTERSECT_PC": return INTERSECT_PC(r1, r2);
                    case "MINUS_IN": return MINUS_IN(r1, r2);
                    case "MINUS_IG": return MINUS_IG(r1, r2);
                    case "MINUS_ME": return MINUS_ME(r1, r2);
                    case "MINUS_PC": return MINUS_PC(r1, r2);
                    case "JOIN_IG": return JOIN_IG(r1, r2);
                    case "JOIN_IN": return JOIN_IN(r1, r2);
                    case "JOIN_ME": return JOIN_ME(r1, r2);
                    case "JOIN_PC": return JOIN_PC(r1, r2);
                }                
            }
            catch (Exception Ex) {}

            return null;
        }

        public static ProbRelation UNION_IN(ProbRelation r1, ProbRelation r2)
            // Phương thức Hợp 2 quan hệ theo chiến lược Independence
        {
            try
            {
                if (!IsUnionCompatible(r1, r2)) throw new Exception("Incompatible Union");
                ProbRelation r = new ProbRelation();
                r.AssignValue(r1);
                bool isCommonTuple;
                foreach (ProbTuple tuple2 in r2.tuples)
                {
                    isCommonTuple = false;
                    // Lấy ra bộ tương ứng trong quan hệ r có cùng khóa với tuple2
                    for (int i = 0; i < r.tuples.Count; i++)
                        if (r.tuples[i].SamePrimaryKeyWith(tuple2))
                        {
                            ProbTuple tuple1 = r.tuples[i];
                            ProbTuple tmpTuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "UNION_IN");
                            if (tmpTuple != null) r.tuples[i] = tmpTuple;
                            isCommonTuple = true;
                            break;
                        }

                    // tuple2 không thuộc quan hệ r ==> add tuple2 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple2);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation UNION_IG(ProbRelation r1, ProbRelation r2)
        // Phương thức Hợp 2 quan hệ theo chiến lược Ignorance
        {
            try
            {
                if (!IsUnionCompatible(r1, r2)) throw new Exception("Incompatible Union");
                ProbRelation r = new ProbRelation();
                r.AssignValue(r1);
                bool isCommonTuple;
                foreach (ProbTuple tuple2 in r2.tuples)
                {
                    isCommonTuple = false;
                    // Lấy ra bộ tương ứng trong quan hệ r có cùng khóa với tuple2
                    for (int i=0; i<r.tuples.Count; i++)
                        if (r.tuples[i].SamePrimaryKeyWith(tuple2))
                        {
                            ProbTuple tuple1 = r.tuples[i];
                            ProbTuple tmpTuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "UNION_IG");
                            if (tmpTuple != null) r.tuples[i] = tmpTuple;
                            isCommonTuple = true;
                            break;
                        }

                    // tuple2 không thuộc quan hệ r ==> add tuple2 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple2);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation UNION_ME(ProbRelation r1, ProbRelation r2)
        // Phương thức Hợp 2 quan hệ theo chiến lược Mutual Exclusion
        {
            try
            {
                if (!IsUnionCompatible(r1, r2)) throw new Exception("Incompatible Union");
                ProbRelation r = new ProbRelation();
                r.AssignValue(r1);
                bool commonTuple;
                foreach (ProbTuple tuple2 in r2.tuples)
                {
                    commonTuple = false;
                    // Lấy ra bộ tương ứng trong quan hệ r có cùng khóa với tuple2
                    for (int i = 0; i < r.tuples.Count; i++)
                        if (r.tuples[i].SamePrimaryKeyWith(tuple2))
                        {
                            ProbTuple tuple1 = r.tuples[i];
                            ProbTuple tmpTuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "UNION_ME");
                            if (tmpTuple != null) r.tuples[i] = tmpTuple;
                            commonTuple = true;
                            break;
                        }

                    // tuple2 không thuộc quan hệ r ==> add tuple2 vào r
                    if (!commonTuple) r.tuples.Add(tuple2);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        
        public static ProbRelation UNION_PC(ProbRelation r1, ProbRelation r2)
        // Phương thức Hợp 2 quan hệ theo chiến lược Positive Correlation
        {
            try
            {
                if (!IsUnionCompatible(r1, r2)) throw new Exception("Incompatible Union");
                ProbRelation r = new ProbRelation();
                r.AssignValue(r1);
                bool isCommonTuple;
                foreach (ProbTuple tuple2 in r2.tuples)
                {
                    isCommonTuple = false;
                    // Lấy ra bộ tương ứng trong quan hệ r có cùng khóa với tuple2
                    for (int i = 0; i < r.tuples.Count; i++)
                        if (r.tuples[i].SamePrimaryKeyWith(tuple2))
                        {
                            ProbTuple tuple1 = r.tuples[i];
                            ProbTuple tmpTuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "UNION_PC");
                            if (tmpTuple != null) r.tuples[i] = tmpTuple;
                            isCommonTuple = true;
                            break;
                        }

                    // tuple2 không thuộc quan hệ r ==> add tuple2 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple2);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        
        public static ProbRelation INTERSECT_IN(ProbRelation r1, ProbRelation r2)
            // Phương thức Giao 2 quan hệ theo chiến lược Independence
        {
            try
            {
                if (!IsIntersectCompatible(r1, r2)) throw new Exception("Incompatible Intersection");
                ProbRelation r = new ProbRelation();
                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            // Sau khi lấy được 2 bộ cùng khóa trên 2 quan hệ khác nhau, ta thực hiện phép Giao trên 2 bộ
                            tuple = (QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "INTERSECT_IN"));
                            if (tuple != null) r.tuples.Add(tuple);
                            break;
                        }
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation INTERSECT_IG(ProbRelation r1, ProbRelation r2)
            // Phương thức Giao 2 quan hệ theo chiến lược Ignorance
        {
            try
            {
                if (!IsIntersectCompatible(r1, r2)) throw new Exception("Incompatible Intersection");
                ProbRelation r = new ProbRelation();
                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            // Sau khi lấy được 2 bộ cùng khóa trên 2 quan hệ khác nhau, ta thực hiện phép Giao trên 2 bộ
                            tuple = (QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "INTERSECT_IG"));
                            if (tuple != null) r.tuples.Add(tuple);
                            break;
                        }
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation INTERSECT_ME(ProbRelation r1, ProbRelation r2)
            // Phương thức Giao 2 quan hệ theo chiến lược Mutual Exclusion
        {
            try
            {
                if (!IsIntersectCompatible(r1, r2)) throw new Exception("Incompatible Intersection");
                ProbRelation r = new ProbRelation();
                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            // Sau khi lấy được 2 bộ cùng khóa trên 2 quan hệ khác nhau, ta thực hiện phép Giao trên 2 bộ
                            tuple = (QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "INTERSECT_ME"));
                            if (tuple != null) r.tuples.Add(tuple);
                            break;
                        }
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation INTERSECT_PC(ProbRelation r1, ProbRelation r2)
            // Phương thức Giao 2 quan hệ theo chiến lược Mutual Positive Correlation
        {
            try
            {
                if (!IsIntersectCompatible(r1, r2)) throw new Exception("Incompatible Intersection");
                ProbRelation r = new ProbRelation();
                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            // Sau khi lấy được 2 bộ cùng khóa trên 2 quan hệ khác nhau, ta thực hiện phép Giao trên 2 bộ
                            tuple = (QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "INTERSECT_PC"));
                            if (tuple != null) r.tuples.Add(tuple);
                            break;
                        }
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation MINUS_IN(ProbRelation r1, ProbRelation r2)
            // Phương thức lấy Hiệu 2 quan hệ theo chiến lược Independence
        {
            try
            {
                if (!IsMinusCompatible(r1, r2)) throw new Exception("Incompatible Minus");
                ProbRelation r = new ProbRelation();

                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                bool isCommonTuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    isCommonTuple = false;
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            isCommonTuple = true;
                            // tuple1 và tuple2 là 2 bộ cùng khóa ==> phải kết hợp 2 bộ bằng phép lấy Hiệu theo chiến lược Independence
                            tuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "MINUS_IN");

                            // Nếu tuple1 MINUS_IN tuple2 là 1 bộ rỗng thì ta không add vào quan hệ kết quả
                            if (tuple != null) r.tuples.Add(tuple);
                        }

                    // tuple1 không thuộc r2 ==> add tuple1 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple1);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation MINUS_IG(ProbRelation r1, ProbRelation r2)
            // Phương thức lấy Hiệu 2 quan hệ theo chiến lược Ignorance
        {
            try
            {
                if (!IsMinusCompatible(r1, r2)) throw new Exception("Incompatible Minus");
                ProbRelation r = new ProbRelation();

                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                bool isCommonTuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    isCommonTuple = false;
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            isCommonTuple = true;
                            // tuple1 và tuple2 là 2 bộ cùng khóa ==> phải kết hợp 2 bộ bằng phép lấy Hiệu theo chiến lược Ignorance
                            tuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "MINUS_IG");

                            // Nếu tuple1 MINUS_IG tuple2 là 1 bộ rỗng thì ta không add vào quan hệ kết quả
                            if (tuple != null) r.tuples.Add(tuple);
                        }

                    // tuple1 không thuộc r2 ==> add tuple1 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple1);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation MINUS_ME(ProbRelation r1, ProbRelation r2)
            // Phương thức lấy Hiệu 2 quan hệ theo chiến lược Mutual Exclusion
        {
            try
            {
                if (!IsMinusCompatible(r1, r2)) throw new Exception("Incompatible Minus");
                ProbRelation r = new ProbRelation();

                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                bool isCommonTuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    isCommonTuple = false;
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            isCommonTuple = true;
                            // tuple1 và tuple2 là 2 bộ cùng khóa ==> phải kết hợp 2 bộ bằng phép lấy Hiệu theo chiến lược Mutual Exclusion
                            tuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "MINUS_ME");

                            // Nếu tuple1 MINUS_ME tuple2 là 1 bộ rỗng thì ta không add vào quan hệ kết quả
                            if (tuple != null) r.tuples.Add(tuple);
                        }

                    // tuple1 không thuộc r2 ==> add tuple1 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple1);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation MINUS_PC(ProbRelation r1, ProbRelation r2)
            // Phương thức lấy Hiệu 2 quan hệ theo chiến lược Positive Correlation
        {
            try
            {
                if (!IsMinusCompatible(r1, r2)) throw new Exception("Incompatible Minus");
                ProbRelation r = new ProbRelation();

                r.scheme.Assign(r1.scheme);
                ProbTuple tuple;
                bool isCommonTuple;
                foreach (ProbTuple tuple1 in r1.tuples)
                {
                    isCommonTuple = false;
                    foreach (ProbTuple tuple2 in r2.tuples)
                        if (tuple1.SamePrimaryKeyWith(tuple2))
                        {
                            isCommonTuple = true;
                            // tuple1 và tuple2 là 2 bộ cùng khóa ==> phải kết hợp 2 bộ bằng phép lấy Hiệu theo chiến lược Positive Correlation
                            tuple = QueryExecution.CombineProbabilisticTuple(tuple1, tuple2, "MINUS_PC");

                            // Nếu tuple1 MINUS_PC tuple2 là 1 bộ rỗng thì ta không add vào quan hệ kết quả
                            if (tuple != null) r.tuples.Add(tuple);
                        }

                    // tuple1 không thuộc r2 ==> add tuple1 vào r
                    if (!isCommonTuple) r.tuples.Add(tuple1);
                }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation JOIN_IG(ProbRelation r1, ProbRelation r2)
            // Phép Kết tự nhiên 2 quan hệ theo chiến lược Ignorance
        {
            try
            {
                if (!IsJoinCompatible(r1, r2)) throw new Exception("Incompatible Join");
                ProbRelation r = new ProbRelation();
                r.scheme.attributes = JoinRelationAttributes(r1.scheme.attributes, r2.scheme.attributes);

                bool check1, check2;
                ProbTuple tuple;
                ProbTriple triple, triple1, triple2;

                foreach (ProbTuple tuple1 in r1.tuples)
                    foreach (ProbTuple tuple2 in r2.tuples)
                    {
                        tuple = new ProbTuple();
                        foreach (ProbAttribute attribute in r.scheme.attributes)
                        {
                            triple = new ProbTriple();
                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r1
                            check1 = r1.scheme.ContainAttribute(attribute);

                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r2
                            check2 = r2.scheme.ContainAttribute(attribute);
                           
                            if (check1 && check2)
                            // Nếu thuộc tính thuộc cả 2 lược đồ => hội 2 giá trị bộ ba xác suất tương ứng của chúng
                            {
                                triple1 = tuple1.GetTriple(attribute);
                                triple2 = tuple2.GetTriple(attribute);
                                triple = ConjTwoTriple(triple1, triple2, "CONJ_IG", attribute.type.typeName);

                                if (triple == null) throw new Exception();
                                else if (triple.values.Count == 0)
                                {
                                    tuple.triples.Clear();
                                    break;
                                }
                                else
                                {
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                            else
                            // Nếu thuộc tính chỉ thuộc một trong hai lược đồ => add bộ ba xác suất tương ứng của thuộc tính vào tuple kết quả
                            {
                                if (check1)
                                    // Trường hợp thuộc tính thuộc lược đồ của quan hệ r1
                                {
                                    triple = tuple1.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                                if (check2)
                                    // Trường hợp thuộc tính thuộc lược đồ của quan hệ r2
                                {
                                    triple = tuple2.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                        }
                        if (tuple.triples.Count == 0) break;
                        else r.tuples.Add(tuple);
                    }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation JOIN_IN(ProbRelation r1, ProbRelation r2)
        // Phép Kết tự nhiên 2 quan hệ theo chiến lược Independence
        {
            try
            {
                if (!IsJoinCompatible(r1, r2)) throw new Exception("Incompatible Join");
                ProbRelation r = new ProbRelation();
                r.scheme.attributes = JoinRelationAttributes(r1.scheme.attributes, r2.scheme.attributes);

                bool check1, check2;
                ProbTuple tuple;
                ProbTriple triple, triple1, triple2;

                foreach (ProbTuple tuple1 in r1.tuples)
                    foreach (ProbTuple tuple2 in r2.tuples)
                    {
                        tuple = new ProbTuple();
                        foreach (ProbAttribute attribute in r.scheme.attributes)
                        {
                            triple = new ProbTriple();
                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r1
                            check1 = r1.scheme.ContainAttribute(attribute);

                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r2
                            check2 = r2.scheme.ContainAttribute(attribute);

                            if (check1 && check2)
                            // Nếu thuộc tính thuộc cả 2 lược đồ => hội 2 giá trị bộ ba xác suất tương ứng của chúng
                            {
                                triple1 = tuple1.GetTriple(attribute);
                                triple2 = tuple2.GetTriple(attribute);
                                triple = ConjTwoTriple(triple1, triple2, "CONJ_IN", attribute.type.typeName);

                                if (triple == null) throw new Exception();
                                else if (triple.values.Count == 0)
                                {
                                    tuple.triples.Clear();
                                    break;
                                }
                                else
                                {
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                            else
                            // Nếu thuộc tính chỉ thuộc một trong hai lược đồ => add bộ ba xác suất tương ứng của thuộc tính vào tuple kết quả
                            {
                                if (check1)
                                // Trường hợp thuộc tính thuộc lược đồ của quan hệ r1
                                {
                                    triple = tuple1.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                                if (check2)
                                // Trường hợp thuộc tính thuộc lược đồ của quan hệ r2
                                {
                                    triple = tuple2.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                        }
                        if (tuple.triples.Count == 0) break;
                        else r.tuples.Add(tuple);
                    }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation JOIN_ME(ProbRelation r1, ProbRelation r2)
        // Phép Kết tự nhiên 2 quan hệ theo chiến lược Mutual Exclusion
        {
            try
            {
                if (!IsJoinCompatible(r1, r2)) throw new Exception("Incompatible Join");
                ProbRelation r = new ProbRelation();
                r.scheme.attributes = JoinRelationAttributes(r1.scheme.attributes, r2.scheme.attributes);

                bool check1, check2;
                ProbTuple tuple;
                ProbTriple triple, triple1, triple2;

                foreach (ProbTuple tuple1 in r1.tuples)
                    foreach (ProbTuple tuple2 in r2.tuples)
                    {
                        tuple = new ProbTuple();
                        foreach (ProbAttribute attribute in r.scheme.attributes)
                        {
                            triple = new ProbTriple();
                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r1
                            check1 = r1.scheme.ContainAttribute(attribute);

                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r2
                            check2 = r2.scheme.ContainAttribute(attribute);

                            if (check1 && check2)
                            // Nếu thuộc tính thuộc cả 2 lược đồ => hội 2 giá trị bộ ba xác suất tương ứng của chúng
                            {
                                triple1 = tuple1.GetTriple(attribute);
                                triple2 = tuple2.GetTriple(attribute);
                                triple = ConjTwoTriple(triple1, triple2, "CONJ_ME", attribute.type.typeName);

                                if (triple == null) throw new Exception();
                                else if (triple.values.Count == 0)
                                {
                                    tuple.triples.Clear();
                                    break;
                                }
                                else
                                {
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                            else
                            // Nếu thuộc tính chỉ thuộc một trong hai lược đồ => add bộ ba xác suất tương ứng của thuộc tính vào tuple kết quả
                            {
                                if (check1)
                                // Trường hợp thuộc tính thuộc lược đồ của quan hệ r1
                                {
                                    triple = tuple1.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                                if (check2)
                                // Trường hợp thuộc tính thuộc lược đồ của quan hệ r2
                                {
                                    triple = tuple2.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                        }
                        if (tuple.triples.Count == 0) break;
                        else r.tuples.Add(tuple);
                    }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbRelation JOIN_PC(ProbRelation r1, ProbRelation r2)
        // Phép Kết tự nhiên 2 quan hệ theo chiến lược Positive Correlation
        {
            try
            {
                if (!IsJoinCompatible(r1, r2)) throw new Exception("Incompatible Join");
                ProbRelation r = new ProbRelation();
                r.scheme.attributes = JoinRelationAttributes(r1.scheme.attributes, r2.scheme.attributes);

                bool check1, check2;
                ProbTuple tuple;
                ProbTriple triple, triple1, triple2;

                foreach (ProbTuple tuple1 in r1.tuples)
                    foreach (ProbTuple tuple2 in r2.tuples)
                    {
                        tuple = new ProbTuple();
                        foreach (ProbAttribute attribute in r.scheme.attributes)
                        {
                            triple = new ProbTriple();
                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r1
                            check1 = r1.scheme.ContainAttribute(attribute);

                            // Kiểm tra xem thuộc tính trên lược đồ của quan hệ kết quả có thuộc lược đồ của quan hệ r2
                            check2 = r2.scheme.ContainAttribute(attribute);

                            if (check1 && check2)
                            // Nếu thuộc tính thuộc cả 2 lược đồ => hội 2 giá trị bộ ba xác suất tương ứng của chúng
                            {
                                triple1 = tuple1.GetTriple(attribute);
                                triple2 = tuple2.GetTriple(attribute);
                                triple = ConjTwoTriple(triple1, triple2, "CONJ_PC", attribute.type.typeName);

                                if (triple == null) throw new Exception();
                                else if (triple.values.Count == 0)
                                {
                                    tuple.triples.Clear();
                                    break;
                                }
                                else
                                {
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                            else
                            // Nếu thuộc tính chỉ thuộc một trong hai lược đồ => add bộ ba xác suất tương ứng của thuộc tính vào tuple kết quả
                            {
                                if (check1)
                                // Trường hợp thuộc tính thuộc lược đồ của quan hệ r1
                                {
                                    triple = tuple1.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                                if (check2)
                                // Trường hợp thuộc tính thuộc lược đồ của quan hệ r2
                                {
                                    triple = tuple2.GetTriple(attribute);
                                    tuple.triples.Add(attribute, triple);
                                    continue;
                                }
                            }
                        }
                        if (tuple.triples.Count == 0) break;
                        else r.tuples.Add(tuple);
                    }
                return r;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static List<ProbAttribute> JoinRelationAttributes(List<ProbAttribute> list1, List<ProbAttribute> list2)
            // Phương thức tạo tập thuộc tính của 2 quan hệ khi Kết
        {
            try
            {
                List<ProbAttribute> listResult = new List<ProbAttribute>();

                // Ban đầu listResult lấy các thuộc tính trong list1
                foreach (ProbAttribute attr1 in list1) listResult.Add(attr1);

                // Xét các thuộc tính trong list2, thuộc tính nào không thuộc listResult => được add vào listResult
                bool commonAttribute;
                foreach (ProbAttribute attr2 in list2)
                {
                    commonAttribute = false;
                    foreach (ProbAttribute attr in listResult)
                        if (attr.IsEqualTo(attr2))
                        {
                            commonAttribute = true;
                            break;
                        }
                    if (!commonAttribute) listResult.Add(attr2);
                }
                return listResult;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static ProbTriple ConjTwoTriple(ProbTriple triple1, ProbTriple triple2, string conjOperator, string dataType)
            // Phương thức Hội 2 bộ ba xác suất
        {
            try
            {
                ProbTriple triple = new ProbTriple();
                for (int i = 0; i < triple1.values.Count; i++)
                    for (int j = 0; j < triple2.values.Count; j++)
                        if (QueryExecution.EQUAL(triple1.values[i].ToString().Trim(), triple2.values[j].ToString().Trim(), dataType))
                        {
                            switch (conjOperator)
                            {
                                case "CONJ_IN":
                                    triple.values.Add(triple1.values[i]);
                                    triple.minprob.Add(triple1.minprob[i] * triple2.minprob[j]);
                                    triple.maxprob.Add(triple1.maxprob[i] * triple2.maxprob[j]);
                                    break;

                                case "CONJ_IG":
                                    triple.values.Add(triple1.values[i]);
                                    triple.minprob.Add(Math.Max(0, triple1.minprob[i] + triple2.minprob[j] - 1));
                                    triple.maxprob.Add(Math.Min(triple1.maxprob[i], triple2.maxprob[j]));
                                    break;

                                case "CONJ_ME":
                                    triple.values.Add(triple1.values[i]);
                                    triple.minprob.Add(0);
                                    triple.maxprob.Add(0);
                                    break;

                                case "CONJ_PC":
                                    triple.values.Add(triple1.values[i]);
                                    triple.minprob.Add(Math.Min(triple1.minprob[i], triple2.minprob[j]));
                                    triple.maxprob.Add(Math.Min(triple1.maxprob[i], triple2.maxprob[j]));
                                    break;

                                default: break;
                            }
                            break;
                        }
                return triple;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public static bool IsJoinCompatible(ProbRelation r1, ProbRelation r2)
            // Phương thức kiểm tra 2 quan hệ có tương thích Kết
        {
            try
            {
                foreach (ProbAttribute attr1 in r1.scheme.attributes)
                {
                    foreach (ProbAttribute attr2 in r2.scheme.attributes)
                        if (attr1.attributeName.Equals(attr2.attributeName))
                            if (!attr1.type.typeName.Equals(attr2.type.typeName))
                                return false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public static bool IsUnionCompatible(ProbRelation r1, ProbRelation r2)
            // Phương thức kiểm tra 2 quan hệ có tương thích Hợp
        {
            try
            {
                if (r1.scheme.attributes.Count != r2.scheme.attributes.Count) return false;
                // Nếu 2 quan hệ có cùng lược đồ => luôn tương thích
                if (r1.scheme == r2.scheme) return true;
                bool checkEqual;
                foreach (ProbAttribute attribute1 in r1.scheme.attributes)
                {
                    checkEqual = false;
                    foreach (ProbAttribute attribute2 in r2.scheme.attributes)
                        // Hai thuộc tính cùng tên, cùng kiểu
                        if (attribute1.IsEqualTo(attribute2))
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

        public static bool IsIntersectCompatible(ProbRelation r1, ProbRelation r2)
            // Phương thức kiểm tra 2 quan hệ có tương thích Giao
        {
            try
            {
                if (r1.scheme.attributes.Count != r2.scheme.attributes.Count) return false;
                // Nếu 2 quan hệ có cùng lược đồ => luôn tương thích
                if (r1.scheme == r2.scheme) return true;
                bool checkEqual;
                foreach (ProbAttribute attribute1 in r1.scheme.attributes)
                {
                    checkEqual = false;
                    foreach (ProbAttribute attribute2 in r2.scheme.attributes)
                        // Hai thuộc tính cùng tên, cùng kiểu
                        if (attribute1.IsEqualTo(attribute2))
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

        public static bool IsMinusCompatible(ProbRelation r1, ProbRelation r2)
            // Phương thức kiểm tra 2 quan hệ có tương thích Hiệu
        {
            try
            {
                if (r1.scheme.attributes.Count != r2.scheme.attributes.Count) return false;
                // Nếu 2 quan hệ có cùng lược đồ => luôn tương thích
                if (r1.scheme == r2.scheme) return true;
                bool checkEqual;
                foreach (ProbAttribute attribute1 in r1.scheme.attributes)
                {
                    checkEqual = false;
                    foreach (ProbAttribute attribute2 in r2.scheme.attributes)
                        // Hai thuộc tính cùng tên, cùng kiểu
                        if (attribute1.IsEqualTo(attribute2))
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
    }
}
