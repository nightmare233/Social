﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Entities;

namespace Social.Domain.DomainServices.FilterExpressions.SystemFiledExpression
{
    public class FromExpression : StringExpression
    {
        public FromExpression() : base("From")
        {
        }

        protected override Expression<Func<Conversation, bool>> Contain(FilterCondition condition)
        {
            return t => t.Messages.Any(m => m.Sender.Name.Contains(condition.Value));
        }

        protected override Expression<Func<Conversation, bool>> NotContain(FilterCondition condition)
        {
            return t => t.Messages.Any(m => !m.Sender.Name.Contains(condition.Value));
        }

        protected override Expression<Func<Conversation, bool>> Is(FilterCondition condition)
        {
            return t => t.Messages.Any(m => m.Sender.Name == condition.Value);
        }

        protected override Expression<Func<Conversation, bool>> IsNot(FilterCondition condition)
        {
            return t => t.Messages.Any(m => m.Sender.Name != condition.Value);
        }
    }
}
