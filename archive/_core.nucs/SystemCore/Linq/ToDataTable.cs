#region License and Terms

// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

namespace MoreLinq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    static partial class MoreEnumerable {
        private static MemberInfo GetAccessedMember(LambdaExpression lambda) {
            var body = lambda.Body;

            // If it's a field access, boxing was used, we need the field
            if ((body.NodeType == ExpressionType.Convert) || (body.NodeType == ExpressionType.ConvertChecked))
                body = ((UnaryExpression) body).Operand;

            // Check if the MemberExpression is valid and is a "first level" member access e.g. not a.b.c 
            var memberExpression = body as MemberExpression;
            if ((memberExpression == null) || (memberExpression.Expression.NodeType != ExpressionType.Parameter))
                throw new ArgumentException(string.Format("Illegal expression: {0}", lambda), "lambda");

            return memberExpression.Member;
        }

        private static IEnumerable<MemberInfo> PrepareMemberInfos<T>(
            ICollection<Expression<Func<T, object>>> expressions) {
            //
            // If no lambda expressions supplied then reflect them off the source element type.
            //

            if (expressions == null || expressions.Count == 0)
                return from m in typeof (T).GetMembers(BindingFlags.Public | BindingFlags.Instance)
                    where m.MemberType == MemberTypes.Field
                          ||
                          (m.MemberType == MemberTypes.Property && ((PropertyInfo) m).GetIndexParameters().Length == 0)
                    select m;

            //
            // Ensure none of the expressions is null.
            //

            if (expressions.Any(e => e == null))
                throw new ArgumentException("One of the supplied expressions was null.", "expressions");

            try {
                return expressions.Select(GetAccessedMember);
            }
            catch (ArgumentException e) {
                throw new ArgumentException("One of the supplied expressions is not allowed.", "expressions", e);
            }
        }
        
        private static UnaryExpression CreateMemberAccessor(Expression parameter, MemberInfo member) {
            var access = Expression.MakeMemberAccess(parameter, member);
            return Expression.Convert(access, typeof (object));
        }

        private static Func<T, object[]> CreateShredder<T>(IEnumerable<MemberInfo> members) {
            var parameter = Expression.Parameter(typeof (T), "e");

            //
            // It is valid for members sequence to have null entries, in
            // which case a null constant is emitted into the corresponding
            // row values array.
            //

            var initializers = members.Select(m => m != null
                ? (Expression) CreateMemberAccessor(parameter, m)
                : Expression.Constant(null, typeof (object)));

            var array = Expression.NewArrayInit(typeof (object), initializers);

            var lambda = Expression.Lambda<Func<T, object[]>>(array, parameter);

            return lambda.Compile();
        }
    }
}