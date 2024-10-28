//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1573, 1591

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace DataModels
{
	/// <summary>
	/// Database       : PVI_ProyectoFinal
	/// Data Source    : Montero\SQLEXPRESS01
	/// Server Version : 16.00.1000
	/// </summary>
	public partial class PviProyectoFinalDB : LinqToDB.Data.DataConnection
	{
		public ITable<Bitacora>     Bitacoras      { get { return this.GetTable<Bitacora>(); } }
		public ITable<Casa>         Casas          { get { return this.GetTable<Casa>(); } }
		public ITable<Categoria>    Categorias     { get { return this.GetTable<Categoria>(); } }
		public ITable<Cobro>        Cobros         { get { return this.GetTable<Cobro>(); } }
		public ITable<DetalleCobro> DetalleCobroes { get { return this.GetTable<DetalleCobro>(); } }
		public ITable<Persona>      Personas       { get { return this.GetTable<Persona>(); } }
		public ITable<Servicio>     Servicios      { get { return this.GetTable<Servicio>(); } }

		public PviProyectoFinalDB()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public PviProyectoFinalDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public PviProyectoFinalDB(DataOptions options)
			: base(options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public PviProyectoFinalDB(DataOptions<PviProyectoFinalDB> options)
			: base(options.Options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table(Schema="dbo", Name="Bitacora")]
	public partial class Bitacora
	{
		[Column("id_bitacora"), PrimaryKey, Identity] public int       IdBitacora { get; set; } // int
		[Column("detalle"),     Nullable            ] public string    Detalle    { get; set; } // varchar(255)
		[Column("id_cobro"),    Nullable            ] public int?      IdCobro    { get; set; } // int
		[Column("id_user"),     Nullable            ] public int?      IdUser     { get; set; } // int
		[Column("fecha"),       Nullable            ] public DateTime? Fecha      { get; set; } // datetime

		#region Associations

		/// <summary>
		/// FK__Cobros__id_bitac__5535A963_BackReference (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdBitacora", OtherKey="IdBitacora", CanBeNull=true)]
		public IEnumerable<Cobro> Cobrosidbitac5535A { get; set; }

		/// <summary>
		/// FK__Bitacora__id_cob__4F7CD00D (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=true)]
		public Cobro Idcob4F7CD00D { get; set; }

		/// <summary>
		/// FK__Bitacora__id_use__5070F446 (dbo.Persona)
		/// </summary>
		[Association(ThisKey="IdUser", OtherKey="IdPersona", CanBeNull=true)]
		public Persona Iduse5070F { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Casas")]
	public partial class Casa
	{
		[Column("id_casa"),             PrimaryKey, Identity] public int      IdCasa             { get; set; } // int
		[Column("nombre_casa"),         NotNull             ] public string   NombreCasa         { get; set; } // nvarchar(255)
		[Column("metros_cuadrados"),    NotNull             ] public int      MetrosCuadrados    { get; set; } // int
		[Column("numero_habitaciones"), NotNull             ] public int      NumeroHabitaciones { get; set; } // int
		[Column("numero_banos"),        NotNull             ] public int      NumeroBanos        { get; set; } // int
		[Column("precio"),              NotNull             ] public decimal  Precio             { get; set; } // decimal(15, 2)
		[Column("id_persona"),          NotNull             ] public int      IdPersona          { get; set; } // int
		[Column("fecha_construccion"),  NotNull             ] public DateTime FechaConstruccion  { get; set; } // date
		[Column("estado"),              NotNull             ] public bool     Estado             { get; set; } // bit

		#region Associations

		/// <summary>
		/// FK__Cobros__id_casa__4CA06362_BackReference (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdCasa", OtherKey="IdCasa", CanBeNull=true)]
		public IEnumerable<Cobro> Cobrosidcasa4Cas { get; set; }

		/// <summary>
		/// FK__Casas__id_person__49C3F6B7 (dbo.Persona)
		/// </summary>
		[Association(ThisKey="IdPersona", OtherKey="IdPersona", CanBeNull=false)]
		public Persona Idperson49C3F6B { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Categorias")]
	public partial class Categoria
	{
		[Column("id_categoria"), PrimaryKey,  Identity] public int    IdCategoria { get; set; } // int
		[Column("nombre"),       NotNull              ] public string Nombre      { get; set; } // varchar(100)
		[Column("descripcion"),     Nullable          ] public string Descripcion { get; set; } // text
		[Column("estado"),       NotNull              ] public bool   Estado      { get; set; } // bit

		#region Associations

		/// <summary>
		/// FK__Servicios__id_ca__3F466844_BackReference (dbo.Servicios)
		/// </summary>
		[Association(ThisKey="IdCategoria", OtherKey="IdCategoria", CanBeNull=true)]
		public IEnumerable<Servicio> Serviciosidca3F { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Cobros")]
	public partial class Cobro
	{
		[Column("id_cobro"),     PrimaryKey,  Identity] public int       IdCobro     { get; set; } // int
		[Column("id_casa"),      NotNull              ] public int       IdCasa      { get; set; } // int
		[Column("mes"),          NotNull              ] public int       Mes         { get; set; } // int
		[Column("anno"),         NotNull              ] public int       Anno        { get; set; } // int
		[Column("id_bitacora"),     Nullable          ] public int?      IdBitacora  { get; set; } // int
		[Column("estado"),          Nullable          ] public string    Estado      { get; set; } // varchar(50)
		[Column("monto"),           Nullable          ] public decimal?  Monto       { get; set; } // decimal(15, 2)
		[Column("fecha_pagada"),    Nullable          ] public DateTime? FechaPagada { get; set; } // date

		#region Associations

		/// <summary>
		/// FK__Bitacora__id_cob__4F7CD00D_BackReference (dbo.Bitacora)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=true)]
		public IEnumerable<Bitacora> Bitacoraidcob4F7CD00D { get; set; }

		/// <summary>
		/// FK__DetalleCo__id_co__5441852A_BackReference (dbo.DetalleCobro)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=true)]
		public IEnumerable<DetalleCobro> DetalleCoidco5441852A { get; set; }

		/// <summary>
		/// FK__Cobros__id_bitac__5535A963 (dbo.Bitacora)
		/// </summary>
		[Association(ThisKey="IdBitacora", OtherKey="IdBitacora", CanBeNull=true)]
		public Bitacora Idbitac5535A { get; set; }

		/// <summary>
		/// FK__Cobros__id_casa__4CA06362 (dbo.Casas)
		/// </summary>
		[Association(ThisKey="IdCasa", OtherKey="IdCasa", CanBeNull=false)]
		public Casa Idcasa4CA { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="DetalleCobro")]
	public partial class DetalleCobro
	{
		[Column("id_servicio"), PrimaryKey(1), NotNull] public int IdServicio { get; set; } // int
		[Column("id_cobro"),    PrimaryKey(2), NotNull] public int IdCobro    { get; set; } // int

		#region Associations

		/// <summary>
		/// FK__DetalleCo__id_co__5441852A (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=false)]
		public Cobro DetalleCoidco5441852A { get; set; }

		/// <summary>
		/// FK__DetalleCo__id_se__534D60F1 (dbo.Servicios)
		/// </summary>
		[Association(ThisKey="IdServicio", OtherKey="IdServicio", CanBeNull=false)]
		public Servicio DetalleCoidse534D60F { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Persona")]
	public partial class Persona
	{
		[Column("id_persona"),       PrimaryKey,  Identity] public int       IdPersona       { get; set; } // int
		[Column("nombre"),           NotNull              ] public string    Nombre          { get; set; } // varchar(100)
		[Column("apellido"),         NotNull              ] public string    Apellido        { get; set; } // varchar(100)
		[Column("email"),            NotNull              ] public string    Email           { get; set; } // varchar(150)
		[Column("telefono"),            Nullable          ] public string    Telefono        { get; set; } // varchar(15)
		[Column("direccion"),           Nullable          ] public string    Direccion       { get; set; } // varchar(255)
		[Column("fecha_nacimiento"),    Nullable          ] public DateTime? FechaNacimiento { get; set; } // date
		[Column("contrasena"),          Nullable          ] public string    Contrasena      { get; set; } // varchar(255)
		[Column("estado"),              Nullable          ] public bool?     Estado          { get; set; } // bit
		[Column("tipo_persona"),        Nullable          ] public string    TipoPersona     { get; set; } // varchar(50)

		#region Associations

		/// <summary>
		/// FK__Bitacora__id_use__5070F446_BackReference (dbo.Bitacora)
		/// </summary>
		[Association(ThisKey="IdPersona", OtherKey="IdUser", CanBeNull=true)]
		public IEnumerable<Bitacora> Bitacoraiduse5070F { get; set; }

		/// <summary>
		/// FK__Casas__id_person__49C3F6B7_BackReference (dbo.Casas)
		/// </summary>
		[Association(ThisKey="IdPersona", OtherKey="IdPersona", CanBeNull=true)]
		public IEnumerable<Casa> Casasidperson49C3F6B { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Servicios")]
	public partial class Servicio
	{
		[Column("id_servicio"),  PrimaryKey,  Identity] public int     IdServicio  { get; set; } // int
		[Column("nombre"),       NotNull              ] public string  Nombre      { get; set; } // varchar(100)
		[Column("descripcion"),     Nullable          ] public string  Descripcion { get; set; } // text
		[Column("precio"),       NotNull              ] public decimal Precio      { get; set; } // decimal(10, 2)
		[Column("id_categoria"), NotNull              ] public int     IdCategoria { get; set; } // int
		[Column("estado"),       NotNull              ] public bool    Estado      { get; set; } // bit

		#region Associations

		/// <summary>
		/// FK__DetalleCo__id_se__534D60F1_BackReference (dbo.DetalleCobro)
		/// </summary>
		[Association(ThisKey="IdServicio", OtherKey="IdServicio", CanBeNull=true)]
		public IEnumerable<DetalleCobro> DetalleCoidse534D60F { get; set; }

		/// <summary>
		/// FK__Servicios__id_ca__3F466844 (dbo.Categorias)
		/// </summary>
		[Association(ThisKey="IdCategoria", OtherKey="IdCategoria", CanBeNull=false)]
		public Categoria Idca3F { get; set; }

		#endregion
	}

	public static partial class PviProyectoFinalDBStoredProcedures
	{
		#region RetornarCasas

		public static IEnumerable<Casa> RetornarCasas(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<Casa>("[dbo].[RetornarCasas]");
		}

		#endregion

		#region RetornarPersonas

		public static IEnumerable<Persona> RetornarPersonas(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<Persona>("[dbo].[RetornarPersonas]");
		}

		#endregion

		#region SpAutenticarUsuario

		public static IEnumerable<SpAutenticarUsuarioResult> SpAutenticarUsuario(this PviProyectoFinalDB dataConnection, string @email, string @contrasena)
		{
			var parameters = new []
			{
				new DataParameter("@email",      @email,      LinqToDB.DataType.VarChar)
				{
					Size = 150
				},
				new DataParameter("@contrasena", @contrasena, LinqToDB.DataType.VarChar)
				{
					Size = 255
				}
			};

			return dataConnection.QueryProc<SpAutenticarUsuarioResult>("[dbo].[SpAutenticarUsuario]", parameters);
		}

		public partial class SpAutenticarUsuarioResult
		{
			[Column("id_persona")  ] public int    Id_persona   { get; set; }
			[Column("nombre")      ] public string Nombre       { get; set; }
			[Column("apellido")    ] public string Apellido     { get; set; }
			[Column("email")       ] public string Email        { get; set; }
			[Column("contrasena")  ] public string Contrasena   { get; set; }
			[Column("tipo_persona")] public string Tipo_persona { get; set; }
			[Column("estado")      ] public bool?  Estado       { get; set; }
		}

		#endregion

		#region SpCrearBitacora

		public static int SpCrearBitacora(this PviProyectoFinalDB dataConnection, int? @idUser, string @detalle, int? @idCobro)
		{
			var parameters = new []
			{
				new DataParameter("@id_user",  @idUser,  LinqToDB.DataType.Int32),
				new DataParameter("@detalle",  @detalle, LinqToDB.DataType.VarChar)
				{
					Size = 255
				},
				new DataParameter("@id_cobro", @idCobro, LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[sp_CrearBitacora]", parameters);
		}

		#endregion

		#region SpCrearCobro

		public static int SpCrearCobro(this PviProyectoFinalDB dataConnection, int? @idUser, int? @idCasa, int? @anno, int? @mes, int? @idServicio)
		{
			var parameters = new []
			{
				new DataParameter("@id_user",     @idUser,     LinqToDB.DataType.Int32),
				new DataParameter("@id_casa",     @idCasa,     LinqToDB.DataType.Int32),
				new DataParameter("@anno",        @anno,       LinqToDB.DataType.Int32),
				new DataParameter("@mes",         @mes,        LinqToDB.DataType.Int32),
				new DataParameter("@id_servicio", @idServicio, LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[sp_CrearCobro]", parameters);
		}

		#endregion

		#region SpModificarCasas

		public static int SpModificarCasas(this PviProyectoFinalDB dataConnection, int? @idCasa, int? @numeroHabitaciones, int? @numeroBanos)
		{
			var parameters = new []
			{
				new DataParameter("@id_casa",             @idCasa,             LinqToDB.DataType.Int32),
				new DataParameter("@numero_habitaciones", @numeroHabitaciones, LinqToDB.DataType.Int32),
				new DataParameter("@numero_banos",        @numeroBanos,        LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[sp_ModificarCasas]", parameters);
		}

		#endregion

		#region SpModificarCobroMensualidades

		public static int SpModificarCobroMensualidades(this PviProyectoFinalDB dataConnection, int? @idCobro, string @estado, string @detalle, int? @idUser)
		{
			var parameters = new []
			{
				new DataParameter("@id_cobro", @idCobro, LinqToDB.DataType.Int32),
				new DataParameter("@estado",   @estado,  LinqToDB.DataType.VarChar)
				{
					Size = 50
				},
				new DataParameter("@detalle",  @detalle, LinqToDB.DataType.VarChar)
				{
					Size = 255
				},
				new DataParameter("@id_user",  @idUser,  LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[sp_ModificarCobroMensualidades]", parameters);
		}

		#endregion

		#region SpObtenerCobros

		public static IEnumerable<SpObtenerCobrosResult> SpObtenerCobros(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<SpObtenerCobrosResult>("[dbo].[sp_ObtenerCobros]");
		}

		public partial class SpObtenerCobrosResult
		{
			[Column("id_cobro")   ] public int    Id_cobro    { get; set; }
			[Column("id_casa")    ] public int    Id_casa     { get; set; }
			[Column("nombre_casa")] public string Nombre_casa { get; set; }
			[Column("id_persona") ] public int    Id_persona  { get; set; }
			[Column("nombre")     ] public string Nombre      { get; set; }
			[Column("apellido")   ] public string Apellido    { get; set; }
			[Column("mes")        ] public int    Mes         { get; set; }
			[Column("anno")       ] public int    Anno        { get; set; }
			[Column("estado")     ] public string Estado      { get; set; }
		}

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Bitacora Find(this ITable<Bitacora> table, int IdBitacora)
		{
			return table.FirstOrDefault(t =>
				t.IdBitacora == IdBitacora);
		}

		public static Casa Find(this ITable<Casa> table, int IdCasa)
		{
			return table.FirstOrDefault(t =>
				t.IdCasa == IdCasa);
		}

		public static Categoria Find(this ITable<Categoria> table, int IdCategoria)
		{
			return table.FirstOrDefault(t =>
				t.IdCategoria == IdCategoria);
		}

		public static Cobro Find(this ITable<Cobro> table, int IdCobro)
		{
			return table.FirstOrDefault(t =>
				t.IdCobro == IdCobro);
		}

		public static DetalleCobro Find(this ITable<DetalleCobro> table, int IdServicio, int IdCobro)
		{
			return table.FirstOrDefault(t =>
				t.IdServicio == IdServicio &&
				t.IdCobro    == IdCobro);
		}

		public static Persona Find(this ITable<Persona> table, int IdPersona)
		{
			return table.FirstOrDefault(t =>
				t.IdPersona == IdPersona);
		}

		public static Servicio Find(this ITable<Servicio> table, int IdServicio)
		{
			return table.FirstOrDefault(t =>
				t.IdServicio == IdServicio);
		}
	}
}
