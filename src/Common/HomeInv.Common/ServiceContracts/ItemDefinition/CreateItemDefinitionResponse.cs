﻿using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class CreateItemDefinitionResponse : BaseResponse
    {
        public ItemDefinitionEntity ItemEntity { get; set; }
    }
}