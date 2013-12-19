﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiDoc.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PosDocumentation")]
	public partial class PosDocumentationDbDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public PosDocumentationDbDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["PosDocumentationConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PosDocumentationDbDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PosDocumentationDbDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PosDocumentationDbDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PosDocumentationDbDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetApiByName")]
		public ISingleResult<GetApiByNameResult> GetApiByName([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string name)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), name);
			return ((ISingleResult<GetApiByNameResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetAPIs")]
		public ISingleResult<GetAPIsResult> GetAPIs()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetAPIsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetHttpVerbs")]
		public ISingleResult<GetHttpVerbsResult> GetHttpVerbs()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetHttpVerbsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMethodById")]
		public ISingleResult<GetMethodByIdResult> GetMethodById([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> methodId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), methodId);
			return ((ISingleResult<GetMethodByIdResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMethodRevisions")]
		public ISingleResult<GetMethodRevisionsResult> GetMethodRevisions([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> moduleId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(100)")] string methodName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), moduleId, methodName);
			return ((ISingleResult<GetMethodRevisionsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMethodsForService")]
		public ISingleResult<GetMethodsForServiceResult> GetMethodsForService([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> serviceId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), serviceId);
			return ((ISingleResult<GetMethodsForServiceResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetModuleByName")]
		public ISingleResult<GetModuleByNameResult> GetModuleByName([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string name, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> apiId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), name, apiId);
			return ((ISingleResult<GetModuleByNameResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetModulesForApi")]
		public ISingleResult<GetModulesForApiResult> GetModulesForApi([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ApiId", DbType="Int")] System.Nullable<int> apiId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), apiId);
			return ((ISingleResult<GetModulesForApiResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMethodByName")]
		public ISingleResult<GetMethodByNameResult> GetMethodByName([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> moduleId, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(100)")] string name, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Int")] System.Nullable<int> revision)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), moduleId, name, revision);
			return ((ISingleResult<GetMethodByNameResult>)(result.ReturnValue));
		}
	}
	
	public partial class GetApiByNameResult
	{
		
		private int _fID;
		
		private string _fApiName;
		
		private string _fDescription;
		
		public GetApiByNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fApiName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fApiName
		{
			get
			{
				return this._fApiName;
			}
			set
			{
				if ((this._fApiName != value))
				{
					this._fApiName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fDescription", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string fDescription
		{
			get
			{
				return this._fDescription;
			}
			set
			{
				if ((this._fDescription != value))
				{
					this._fDescription = value;
				}
			}
		}
	}
	
	public partial class GetAPIsResult
	{
		
		private int _fID;
		
		private string _fApiName;
		
		private string _fDescription;
		
		public GetAPIsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fApiName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fApiName
		{
			get
			{
				return this._fApiName;
			}
			set
			{
				if ((this._fApiName != value))
				{
					this._fApiName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fDescription", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string fDescription
		{
			get
			{
				return this._fDescription;
			}
			set
			{
				if ((this._fDescription != value))
				{
					this._fDescription = value;
				}
			}
		}
	}
	
	public partial class GetHttpVerbsResult
	{
		
		private int _fID;
		
		private string _fHttpVerb;
		
		public GetHttpVerbsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fHttpVerb", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fHttpVerb
		{
			get
			{
				return this._fHttpVerb;
			}
			set
			{
				if ((this._fHttpVerb != value))
				{
					this._fHttpVerb = value;
				}
			}
		}
	}
	
	public partial class GetMethodByIdResult
	{
		
		private int _fID;
		
		private int _frServiceId;
		
		private string _fMethodName;
		
		private int _frHttpVerb;
		
		private bool _fRequiresAuthentication;
		
		private bool _fRequiresAuthorization;
		
		private string _fRequestSample;
		
		private string _fResponseSample;
		
		private string _fDescription;
		
		public GetMethodByIdResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_frServiceId", DbType="Int NOT NULL")]
		public int frServiceId
		{
			get
			{
				return this._frServiceId;
			}
			set
			{
				if ((this._frServiceId != value))
				{
					this._frServiceId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fMethodName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string fMethodName
		{
			get
			{
				return this._fMethodName;
			}
			set
			{
				if ((this._fMethodName != value))
				{
					this._fMethodName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_frHttpVerb", DbType="Int NOT NULL")]
		public int frHttpVerb
		{
			get
			{
				return this._frHttpVerb;
			}
			set
			{
				if ((this._frHttpVerb != value))
				{
					this._frHttpVerb = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRequiresAuthentication", DbType="Bit NOT NULL")]
		public bool fRequiresAuthentication
		{
			get
			{
				return this._fRequiresAuthentication;
			}
			set
			{
				if ((this._fRequiresAuthentication != value))
				{
					this._fRequiresAuthentication = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRequiresAuthorization", DbType="Bit NOT NULL")]
		public bool fRequiresAuthorization
		{
			get
			{
				return this._fRequiresAuthorization;
			}
			set
			{
				if ((this._fRequiresAuthorization != value))
				{
					this._fRequiresAuthorization = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRequestSample", DbType="VarChar(MAX)")]
		public string fRequestSample
		{
			get
			{
				return this._fRequestSample;
			}
			set
			{
				if ((this._fRequestSample != value))
				{
					this._fRequestSample = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fResponseSample", DbType="VarChar(MAX)")]
		public string fResponseSample
		{
			get
			{
				return this._fResponseSample;
			}
			set
			{
				if ((this._fResponseSample != value))
				{
					this._fResponseSample = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fDescription", DbType="VarChar(MAX) NOT NULL", CanBeNull=false)]
		public string fDescription
		{
			get
			{
				return this._fDescription;
			}
			set
			{
				if ((this._fDescription != value))
				{
					this._fDescription = value;
				}
			}
		}
	}
	
	public partial class GetMethodRevisionsResult
	{
		
		private System.DateTime _fChangeDate;
		
		private string _fAuthor;
		
		private System.Nullable<long> _fRevisionNumber;
		
		public GetMethodRevisionsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fChangeDate", DbType="DateTime NOT NULL")]
		public System.DateTime fChangeDate
		{
			get
			{
				return this._fChangeDate;
			}
			set
			{
				if ((this._fChangeDate != value))
				{
					this._fChangeDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fAuthor", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fAuthor
		{
			get
			{
				return this._fAuthor;
			}
			set
			{
				if ((this._fAuthor != value))
				{
					this._fAuthor = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRevisionNumber", DbType="BigInt")]
		public System.Nullable<long> fRevisionNumber
		{
			get
			{
				return this._fRevisionNumber;
			}
			set
			{
				if ((this._fRevisionNumber != value))
				{
					this._fRevisionNumber = value;
				}
			}
		}
	}
	
	public partial class GetMethodsForServiceResult
	{
		
		private int _fID;
		
		private string _fMethodName;
		
		private int _frHttpVerb;
		
		public GetMethodsForServiceResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fMethodName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string fMethodName
		{
			get
			{
				return this._fMethodName;
			}
			set
			{
				if ((this._fMethodName != value))
				{
					this._fMethodName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_frHttpVerb", DbType="Int NOT NULL")]
		public int frHttpVerb
		{
			get
			{
				return this._frHttpVerb;
			}
			set
			{
				if ((this._frHttpVerb != value))
				{
					this._frHttpVerb = value;
				}
			}
		}
	}
	
	public partial class GetModuleByNameResult
	{
		
		private int _fID;
		
		private string _fServiceName;
		
		public GetModuleByNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fServiceName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fServiceName
		{
			get
			{
				return this._fServiceName;
			}
			set
			{
				if ((this._fServiceName != value))
				{
					this._fServiceName = value;
				}
			}
		}
	}
	
	public partial class GetModulesForApiResult
	{
		
		private int _fID;
		
		private string _fServiceName;
		
		public GetModulesForApiResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fServiceName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fServiceName
		{
			get
			{
				return this._fServiceName;
			}
			set
			{
				if ((this._fServiceName != value))
				{
					this._fServiceName = value;
				}
			}
		}
	}
	
	public partial class GetMethodByNameResult
	{
		
		private int _fID;
		
		private int _frServiceId;
		
		private string _fMethodName;
		
		private int _frHttpVerb;
		
		private bool _fRequiresAuthentication;
		
		private bool _fRequiresAuthorization;
		
		private string _fRequestSample;
		
		private string _fResponseSample;
		
		private string _fDescription;
		
		private System.DateTime _fChangeDate;
		
		private string _fAuthor;
		
		private System.Nullable<int> _fRevisionNumber;
		
		public GetMethodByNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fID", DbType="Int NOT NULL")]
		public int fID
		{
			get
			{
				return this._fID;
			}
			set
			{
				if ((this._fID != value))
				{
					this._fID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_frServiceId", DbType="Int NOT NULL")]
		public int frServiceId
		{
			get
			{
				return this._frServiceId;
			}
			set
			{
				if ((this._frServiceId != value))
				{
					this._frServiceId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fMethodName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string fMethodName
		{
			get
			{
				return this._fMethodName;
			}
			set
			{
				if ((this._fMethodName != value))
				{
					this._fMethodName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_frHttpVerb", DbType="Int NOT NULL")]
		public int frHttpVerb
		{
			get
			{
				return this._frHttpVerb;
			}
			set
			{
				if ((this._frHttpVerb != value))
				{
					this._frHttpVerb = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRequiresAuthentication", DbType="Bit NOT NULL")]
		public bool fRequiresAuthentication
		{
			get
			{
				return this._fRequiresAuthentication;
			}
			set
			{
				if ((this._fRequiresAuthentication != value))
				{
					this._fRequiresAuthentication = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRequiresAuthorization", DbType="Bit NOT NULL")]
		public bool fRequiresAuthorization
		{
			get
			{
				return this._fRequiresAuthorization;
			}
			set
			{
				if ((this._fRequiresAuthorization != value))
				{
					this._fRequiresAuthorization = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRequestSample", DbType="NVarChar(MAX)")]
		public string fRequestSample
		{
			get
			{
				return this._fRequestSample;
			}
			set
			{
				if ((this._fRequestSample != value))
				{
					this._fRequestSample = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fResponseSample", DbType="NVarChar(MAX)")]
		public string fResponseSample
		{
			get
			{
				return this._fResponseSample;
			}
			set
			{
				if ((this._fResponseSample != value))
				{
					this._fResponseSample = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fDescription", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string fDescription
		{
			get
			{
				return this._fDescription;
			}
			set
			{
				if ((this._fDescription != value))
				{
					this._fDescription = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fChangeDate", DbType="DateTime NOT NULL")]
		public System.DateTime fChangeDate
		{
			get
			{
				return this._fChangeDate;
			}
			set
			{
				if ((this._fChangeDate != value))
				{
					this._fChangeDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fAuthor", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string fAuthor
		{
			get
			{
				return this._fAuthor;
			}
			set
			{
				if ((this._fAuthor != value))
				{
					this._fAuthor = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fRevisionNumber", DbType="Int")]
		public System.Nullable<int> fRevisionNumber
		{
			get
			{
				return this._fRevisionNumber;
			}
			set
			{
				if ((this._fRevisionNumber != value))
				{
					this._fRevisionNumber = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
