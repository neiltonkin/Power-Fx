﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerFx.Core.Entities;
using Microsoft.PowerFx.Core.Entities.Delegation;
using Microsoft.PowerFx.Core.Functions.Delegation;
using Microsoft.PowerFx.Core.Functions.Delegation.DelegationMetadata;
using Microsoft.PowerFx.Core.Tests.Helpers;
using Microsoft.PowerFx.Core.Types;
using Microsoft.PowerFx.Core.Utils;

namespace Microsoft.PowerFx.Core.Tests.AssociatedDataSourcesTests
{
    public class AccountsEntity : IExternalEntity, IExternalDataSource
    {
        public DName EntityName => new DName("Accounts");

        public string Name => "Accounts";

        public bool IsSelectable => true;

        public bool IsDelegatable => true;

        public bool IsRefreshable => true;

        public bool RequiresAsync => true;

        public bool IsPageable => true;

        DType IExternalEntity.Type => AccountsTypeHelper.GetDType();

        IExternalDataEntityMetadataProvider IExternalDataSource.DataEntityMetadataProvider => throw new NotImplementedException();

        DataSourceKind IExternalDataSource.Kind => throw new NotImplementedException();

        IExternalTableMetadata IExternalDataSource.TableMetadata => throw new NotImplementedException();

        IDelegationMetadata IExternalDataSource.DelegationMetadata => new AccountsDelegationMetadata();
    }

    internal static class AccountsTypeHelper
    {
        internal const string SimplifiedAccountsSchema = "*[accountid:g, accountnumber:s, address1_addresstypecode:l, address1_city:s, address1_composite:s, address1_country:s, address1_county:s, address1_fax:s, address1_freighttermscode:l, address1_latitude:n, address1_line1:s, address1_line2:s, address1_line3:s, address1_longitude:n, address1_name:s, address1_postalcode:s, address1_postofficebox:s, address1_primarycontactname:s, address1_shippingmethodcode:l, address1_stateorprovince:s, address1_telephone1:s, address1_telephone2:s, address1_telephone3:s, address1_upszone:s, address1_utcoffset:n, createdon:d, description:s, emailaddress1:s, emailaddress2:s, emailaddress3:s, modifiedon:d, name:s, numberofemployees:n, primarytwitterid:s, stockexchange:s, telephone1:s, telephone2:s, telephone3:s, tickersymbol:s, versionnumber:n, websiteurl:h]";

        public static DType GetDType()
        {
            DType accountsType = TestUtils.DT(SimplifiedAccountsSchema);
            var dataSource = new TestDataSource(
                "Accounts", 
                accountsType, 
                keyColumns: new[] { "accountid" },
                selectableColumns: new[] { "name", "address1_city", "accountid", "address1_country", "address1_line1" });
            var displayNameMapping = dataSource.DisplayNameMapping;
            displayNameMapping.Add("name", "Account Name");
            displayNameMapping.Add("address1_city", "Address 1: City");
            displayNameMapping.Add("address1_line1", "Address 1: Street 1");
            return DType.AttachDataSourceInfo(accountsType, dataSource);
        }
    }

    internal class AccountsDelegationMetadata : IDelegationMetadata
    {
        public DType Schema => AccountsTypeHelper.GetDType();

        public DelegationCapability TableAttributes => throw new NotImplementedException();

        public DelegationCapability TableCapabilities => new DelegationCapability(DelegationCapability.SupportsAll);

        public SortOpMetadata SortDelegationMetadata => new SortOpMetadata(Schema, new Dictionary<DPath, DelegationCapability>());

        public FilterOpMetadata FilterDelegationMetadata { get; }

        public GroupOpMetadata GroupDelegationMetadata => throw new NotImplementedException();

        public Dictionary<DPath, DPath> ODataPathReplacementMap => throw new NotImplementedException();

        public AccountsDelegationMetadata()
        {
            FilterDelegationMetadata = new FilterOpMetadata(
                Schema,
                new Dictionary<DPath, DelegationCapability>(),
                new Dictionary<DPath, DelegationCapability>(),
                DelegationCapability.SupportsAll,
                DelegationCapability.SupportsAll);
        }
    }

    internal class TestDVEntity
    {
        private class MyExpandInfo : IExpandInfo
        {
            public string Identity => "An identity";

            public bool IsTable => true;

            public string Name => "Name";

            public string PolymorphicParent => throw new NotImplementedException();

            public IExternalDataSource ParentDataSource => throw new NotImplementedException();

            public ExpandPath ExpandPath => throw new NotImplementedException();

            public IExpandInfo Clone()
            {
                throw new NotImplementedException();
            }

            public string ToDebugString()
            {
                throw new NotImplementedException();
            }

            public void UpdateEntityInfo(IExternalDataSource dataSource, string relatedEntityPath)
            {
                throw new NotImplementedException();
            }
        }

        public static DType Create()
        {
            return DType.CreateExpandType(new MyExpandInfo());
        }
    }
}
