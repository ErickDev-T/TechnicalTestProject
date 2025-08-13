using System;
using System.Collections.Generic;

//code to connect to data base
//Scaffold-DbContext "Data Source=DESKTOP-372NBM0\RIC;Database=testDGADB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
using ProjectAPIStore.Data;


namespace ProjectAPIStore.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Username { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
