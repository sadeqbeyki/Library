﻿using AppFramework.Domain;
using LMS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.TranslatorAgg
{
    public interface ITranslatorRepository : IRepository<Translator>
    {
        //TranslatorDto Add(TranslatorDto dto);
    }
}
