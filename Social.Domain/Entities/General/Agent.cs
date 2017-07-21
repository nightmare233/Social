﻿using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Entities.General
{
    public class Agent:Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
