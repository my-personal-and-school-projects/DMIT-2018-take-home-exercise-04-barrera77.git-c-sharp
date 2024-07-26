﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TakeHomeExercise4System.Entities;

public partial class StoreRefund
{
    [Key]
    [Column("RefundID")]
    public int RefundId { get; set; }

    [Column("InvoiceID")]
    public int InvoiceId { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    [Column("OriginalInvoiceID")]
    public int OriginalInvoiceId { get; set; }

    [Required]
    [StringLength(150)]
    public string Reason { get; set; }

    public bool RemoveFromViewFlag { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("StoreRefundInvoices")]
    public virtual Invoice Invoice { get; set; }

    [ForeignKey("OriginalInvoiceId")]
    [InverseProperty("StoreRefundOriginalInvoices")]
    public virtual Invoice OriginalInvoice { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("StoreRefunds")]
    public virtual Product Product { get; set; }
}