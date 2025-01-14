﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using back_dotnet.Models.Domain;

#nullable disable

namespace back_dotnet.Migrations
{
    [DbContext(typeof(HairunSiContext))]
    partial class HairunSiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("back_dotnet.Models.Domain.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("IdRole")
                        .HasColumnType("uuid")
                        .HasColumnName("id_role");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("PK_839517a681a86bb84cbcc6a1e9d");

                    b.HasIndex("IdRole");

                    b.HasIndex(new[] { "Name" }, "UQ_8681da666ad9699d568b3e91064")
                        .IsUnique();

                    b.HasIndex(new[] { "Name" }, "name_department_unique")
                        .IsUnique();

                    b.ToTable("departments", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Discussion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("description");

                    b.Property<int>("ParticipantNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("participant_number")
                        .HasDefaultValueSql("2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("PK_Discussion");

                    b.HasIndex(new[] { "Title" }, "UQ_discussion_title")
                        .IsUnique();

                    b.ToTable("discussion", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("path");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("public_id");

                    b.Property<string>("Size")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("size");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("PK_6c16b9093a142e0e7613b04a3d9");

                    b.HasIndex(new[] { "Name" }, "UQ_332d10755187ac3c580e21fbc02")
                        .IsUnique();

                    b.ToTable("files", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "file");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Leave", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("end_date");

                    b.Property<Guid>("IdLeaveType")
                        .HasColumnType("uuid")
                        .HasColumnName("id_leave_type");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid")
                        .HasColumnName("id_user");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("start_date");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("PK_Leave");

                    b.HasIndex("IdLeaveType");

                    b.HasIndex("IdUser");

                    b.ToTable("leave", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.LeaveAuthorization", b =>
                {
                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid")
                        .HasColumnName("id_user");

                    b.Property<Guid>("IdLeaveType")
                        .HasColumnType("uuid")
                        .HasColumnName("id_leave_type");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("EndValidity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("end_validity")
                        .HasDefaultValueSql("now() + interval '1 year'");

                    b.Property<decimal>("Solde")
                        .HasColumnType("numeric(5, 2)")
                        .HasColumnName("solde");

                    b.Property<DateTime>("StartValidity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("start_validity")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("IdUser", "IdLeaveType")
                        .HasName("PK_User_Leave");

                    b.HasIndex("IdLeaveType");

                    b.ToTable("leave_authorization", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.LeaveType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<bool>("IsCumulable")
                        .HasColumnType("boolean")
                        .HasColumnName("is_cumulable");

                    b.Property<decimal>("SoldeEachYear")
                        .HasColumnType("numeric(5, 2)")
                        .HasColumnName("solde_each_year");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("PK_Leave_type");

                    b.HasIndex(new[] { "Designation" }, "UQ_leave_type_designation_unique")
                        .IsUnique();

                    b.ToTable("leave_type", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("IdDiscussion")
                        .HasColumnType("uuid")
                        .HasColumnName("id_discussion");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid")
                        .HasColumnName("id_user");

                    b.HasKey("Id")
                        .HasName("PK_Message");

                    b.HasIndex("IdDiscussion");

                    b.HasIndex("IdUser");

                    b.ToTable("message", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.MessageState", b =>
                {
                    b.Property<Guid>("IdMessage")
                        .HasColumnType("uuid")
                        .HasColumnName("id_message");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid")
                        .HasColumnName("id_user");

                    b.Property<bool>("IsRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_read");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("read_at");

                    b.HasKey("IdMessage", "IdUser")
                        .HasName("PK_Message_State");

                    b.HasIndex("IdUser");

                    b.ToTable("message_state", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Migration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("PK_8c82d7f526340ab734260ea46be");

                    b.ToTable("migrations", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("PK_920331560282b8bd21bb02290df");

                    b.HasIndex(new[] { "Name" }, "UQ_48ce552495d14eae9b187bb6716")
                        .IsUnique();

                    b.HasIndex(new[] { "Name" }, "name_permission_unique")
                        .IsUnique();

                    b.ToTable("permissions", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.PermissionRole", b =>
                {
                    b.Property<Guid>("IdRole")
                        .HasColumnType("uuid")
                        .HasColumnName("id_role");

                    b.Property<Guid>("IdPermission")
                        .HasColumnType("uuid")
                        .HasColumnName("id_permission");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.HasKey("IdRole", "IdPermission")
                        .HasName("PK_be2cc366c36c67d6052b751b94f");

                    b.HasIndex("IdPermission");

                    b.ToTable("permission_role", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("IdDepartment")
                        .HasColumnType("uuid")
                        .HasColumnName("id_department");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("PK_2829ac61eff60fcec60d7274b9e");

                    b.HasIndex("IdDepartment");

                    b.HasIndex(new[] { "Name" }, "UQ_af95ddf25e9bd491236781b1aef")
                        .IsUnique();

                    b.HasIndex(new[] { "Name" }, "name_post_unique")
                        .IsUnique();

                    b.ToTable("posts", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "post");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("PK_c1433d71a4838793a49dcad46ab");

                    b.HasIndex(new[] { "Name" }, "UQ_648e3f5447f725579d7d4ffdfb7")
                        .IsUnique();

                    b.HasIndex(new[] { "Name" }, "name_role_unique")
                        .IsUnique();

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.User", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("birth_date");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email")
                        .HasDefaultValueSql("'example@hairun-technology.com'::character varying");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("firstname");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("IdFile")
                        .HasColumnType("uuid")
                        .HasColumnName("id_file");

                    b.Property<Guid>("IdPost")
                        .HasColumnType("uuid")
                        .HasColumnName("id_post");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("lastname");

                    b.Property<string>("Matricule")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("character varying")
                        .HasColumnName("matricule")
                        .HasComputedColumnSql("\nCASE\n    WHEN (length((id)::text) >= 4) THEN ('M'::text || \"substring\"(('00'::text || id), '.{LENGTH(id::text)}$'::text))\n    ELSE ('M'::text || \"substring\"(('00'::text || id), '.{3}$'::text))\nEND", true);

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Uuid");

                    b.HasIndex("IdFile");

                    b.HasIndex("IdPost");

                    b.HasIndex(new[] { "Email" }, "email_user_unique")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.UserDiscussion", b =>
                {
                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid")
                        .HasColumnName("id_user");

                    b.Property<Guid>("IdDiscussion")
                        .HasColumnType("uuid")
                        .HasColumnName("id_discussion");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("IdUser", "IdDiscussion")
                        .HasName("PK_User_Discussion");

                    b.HasIndex("IdDiscussion");

                    b.ToTable("user_discussion", (string)null);
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Department", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.Role", "IdRoleNavigation")
                        .WithMany("Departments")
                        .HasForeignKey("IdRole")
                        .IsRequired()
                        .HasConstraintName("FK_3dfe737787c5daa45d0b3b7242c");

                    b.Navigation("IdRoleNavigation");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Leave", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.LeaveType", "Type")
                        .WithMany()
                        .HasForeignKey("IdLeaveType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Leave_LeaveType");

                    b.HasOne("back_dotnet.Models.Domain.User", "User")
                        .WithMany("Leaves")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Leave_User");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.LeaveAuthorization", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.LeaveType", "LeaveType")
                        .WithMany("LeavesAuthorization")
                        .HasForeignKey("IdLeaveType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_LeaveAuthorization_LeaveType");

                    b.HasOne("back_dotnet.Models.Domain.User", "User")
                        .WithMany("LeavesAuthorization")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_LeaveAuthorization_User");

                    b.Navigation("LeaveType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Message", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.Discussion", "Discussion")
                        .WithMany("Messages")
                        .HasForeignKey("IdDiscussion")
                        .IsRequired()
                        .HasConstraintName("FK_Message_Discussion");

                    b.HasOne("back_dotnet.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Message_User");

                    b.Navigation("Discussion");

                    b.Navigation("User");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.MessageState", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.Message", "Message")
                        .WithMany("MessageStates")
                        .HasForeignKey("IdMessage")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Message_State_Message");

                    b.HasOne("back_dotnet.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Message_State_User");

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.PermissionRole", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.Permission", "IdPermissionNavigation")
                        .WithMany("PermissionRoles")
                        .HasForeignKey("IdPermission")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_756cb410f074c6f61837ea4580d");

                    b.HasOne("back_dotnet.Models.Domain.Role", "IdRoleNavigation")
                        .WithMany("PermissionRoles")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_4206b87fef57d4d8e3beb58efe1");

                    b.Navigation("IdPermissionNavigation");

                    b.Navigation("IdRoleNavigation");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Post", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.Department", "IdDepartmentNavigation")
                        .WithMany("Posts")
                        .HasForeignKey("IdDepartment")
                        .IsRequired()
                        .HasConstraintName("FK_ff5f0f4c0b547c6e6eb88ae4c1f");

                    b.Navigation("IdDepartmentNavigation");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.User", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.File", "IdFileNavigation")
                        .WithMany()
                        .HasForeignKey("IdFile")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_7b29386038a3e512a8744b8c227");

                    b.HasOne("back_dotnet.Models.Domain.Post", "IdPostNavigation")
                        .WithMany()
                        .HasForeignKey("IdPost")
                        .IsRequired()
                        .HasConstraintName("FK_1bf3c8c1ba60d51dd3e84624010");

                    b.Navigation("IdFileNavigation");

                    b.Navigation("IdPostNavigation");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.UserDiscussion", b =>
                {
                    b.HasOne("back_dotnet.Models.Domain.Discussion", "Discussion")
                        .WithMany("UserDiscussions")
                        .HasForeignKey("IdDiscussion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_User_Discussion_Discussion");

                    b.HasOne("back_dotnet.Models.Domain.User", "User")
                        .WithMany("UserDiscussions")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_User_Discussion_User");

                    b.Navigation("Discussion");

                    b.Navigation("User");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Department", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Discussion", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UserDiscussions");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.LeaveType", b =>
                {
                    b.Navigation("LeavesAuthorization");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Message", b =>
                {
                    b.Navigation("MessageStates");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Permission", b =>
                {
                    b.Navigation("PermissionRoles");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.Role", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("PermissionRoles");
                });

            modelBuilder.Entity("back_dotnet.Models.Domain.User", b =>
                {
                    b.Navigation("Leaves");

                    b.Navigation("LeavesAuthorization");

                    b.Navigation("UserDiscussions");
                });
#pragma warning restore 612, 618
        }
    }
}
