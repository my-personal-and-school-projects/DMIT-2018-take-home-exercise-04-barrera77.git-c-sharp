﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TakeHomeExercise4System.Entities;

[PrimaryKey("ProductId", "EmployeeId")]
public partial class SalesCartItem
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public bool RemoveFromViewFlag { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("SalesCartItems")]
    public virtual Employee Employee { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("SalesCartItems")]
    public virtual Product Product { get; set; }
}