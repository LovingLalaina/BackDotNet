using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Models.Domain;

public partial class HairunSiContext : DbContext
{
    public HairunSiContext()
    {
    }

    public HairunSiContext(DbContextOptions<HairunSiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionRole> PermissionRoles { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Leave> Leave { get; set; }

    public virtual DbSet<LeaveType> LeaveType { get; set; }

    public virtual DbSet<LeaveAuthorization> LeaveAuthorization { get; set; }

    public virtual DbSet<Discussion> Discussions { get; set; }

    public virtual DbSet<UserDiscussion> UserDiscussions { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageState> MessagesState { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_839517a681a86bb84cbcc6a1e9d");

            entity.ToTable("departments");

            entity.HasIndex(e => e.Name, "UQ_8681da666ad9699d568b3e91064").IsUnique();

            entity.HasIndex(e => e.Name, "name_department_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_3dfe737787c5daa45d0b3b7242c");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_6c16b9093a142e0e7613b04a3d9");

            entity.ToTable("files");

            entity.HasIndex(e => e.Name, "UQ_332d10755187ac3c580e21fbc02").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .HasColumnName("path");
            entity.Property(e => e.PublicId)
                .HasMaxLength(255)
                .HasColumnName("public_id");
            entity.Property(e => e.Size)
                .HasMaxLength(255)
                .HasColumnName("size");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_8c82d7f526340ab734260ea46be");

            entity.ToTable("migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_920331560282b8bd21bb02290df");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Name, "UQ_48ce552495d14eae9b187bb6716").IsUnique();

            entity.HasIndex(e => e.Name, "name_permission_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PermissionRole>(entity =>
        {
            entity.HasKey(e => new { e.IdRole, e.IdPermission }).HasName("PK_be2cc366c36c67d6052b751b94f");

            entity.ToTable("permission_role");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IdPermission).HasColumnName("id_permission");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdPermissionNavigation).WithMany(p => p.PermissionRoles)
                .HasForeignKey(d => d.IdPermission)
                .HasConstraintName("FK_756cb410f074c6f61837ea4580d");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.PermissionRoles)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_4206b87fef57d4d8e3beb58efe1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_2829ac61eff60fcec60d7274b9e");

            entity.ToTable("posts");

            entity.HasIndex(e => e.Name, "UQ_af95ddf25e9bd491236781b1aef").IsUnique();

            entity.HasIndex(e => e.Name, "name_post_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IdDepartment).HasColumnName("id_department");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdDepartment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ff5f0f4c0b547c6e6eb88ae4c1f");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_c1433d71a4838793a49dcad46ab");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "UQ_648e3f5447f725579d7d4ffdfb7").IsUnique();

            entity.HasIndex(e => e.Name, "name_role_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasKey(e => e.Uuid);
            
            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email_user_unique").IsUnique();

            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birth_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("'example@hairun-technology.com'::character varying")
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasColumnType("character varying")
                .HasColumnName("firstname");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdFile).HasColumnName("id_file");
            entity.Property(e => e.IdPost).HasColumnName("id_post");
            entity.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("lastname");
            entity.Property(e => e.Matricule)
                .HasComputedColumnSql("\nCASE\n    WHEN (length((id)::text) >= 4) THEN ('M'::text || \"substring\"(('00'::text || id), '.{LENGTH(id::text)}$'::text))\n    ELSE ('M'::text || \"substring\"(('00'::text || id), '.{3}$'::text))\nEND", true)
                .HasColumnType("character varying")
                .HasColumnName("matricule");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("uuid");

            entity.HasOne(d => d.IdFileNavigation).WithMany()
                .HasForeignKey(d => d.IdFile)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_7b29386038a3e512a8744b8c227");

            entity.HasOne(d => d.IdPostNavigation).WithMany()
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_1bf3c8c1ba60d51dd3e84624010");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.ToTable("leave");
            entity.HasKey(e => e.Id).HasName("PK_Leave");
            entity.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasColumnName("id")
                .HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.Status)
                .HasConversion<int>()
                .HasColumnName("status");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description")
                .HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.Property(leave => leave.IdUser)
                .HasColumnType("uuid")
                .HasColumnName("id_user");
            entity.HasOne(leave => leave.User).WithMany(user => user.Leaves)
                .HasForeignKey(leave => leave.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Leave_User");
            entity.Property(leave => leave.IdLeaveType)
                .HasColumnType("uuid")
                .HasColumnName("id_leave_type");
            entity.HasOne(leave => leave.Type).WithMany()
                .HasForeignKey(leave => leave.IdLeaveType)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Leave_LeaveType");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.ToTable("leave_type");
            entity.HasKey(e => e.Id).HasName("PK_Leave_type");
            entity.Property(e => e.Id)
                .HasColumnType("uuid")
                .HasColumnName("id")
                .HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Designation)
                .HasColumnType("character varying")
                .HasColumnName("designation")
                .HasMaxLength(255);
            entity.HasIndex(e => e.Designation, "UQ_leave_type_designation_unique").IsUnique();
            entity.Property(e => e.IsCumulable)
                .HasColumnType("boolean")
                .HasColumnName("is_cumulable");
            entity.Property(e => e.SoldeEachYear)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("solde_each_year");
             entity.Property(e => e.Designation)
                .HasColumnType("character varying")
                .HasColumnName("description")
                .HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<LeaveAuthorization>(entity =>
        {
            entity.ToTable("leave_authorization");
            entity.Property(e => e.Solde)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("solde");
            entity.Property(e => e.StartValidity)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_validity")
                .HasDefaultValueSql("now()");
            entity.Property(e => e.EndValidity)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_validity")
                .HasDefaultValueSql("now() + interval '1 year'");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasKey(leaveAuthorization => new { leaveAuthorization.IdUser, leaveAuthorization.IdLeaveType})
                .HasName("PK_User_Leave");
                
            entity.Property(leaveAuthorization => leaveAuthorization.IdUser)
                .HasColumnType("uuid")
                .HasColumnName("id_user");
            entity.HasOne(leaveAuthorization => leaveAuthorization.User).WithMany(user => user.LeavesAuthorization)
                .HasForeignKey(leaveAuthorization => leaveAuthorization.IdUser)
                .HasConstraintName("FK_LeaveAuthorization_User");

            entity.Property(leaveAuthorization => leaveAuthorization.IdLeaveType)
                .HasColumnType("uuid")
                .HasColumnName("id_leave_type");
            entity.HasOne(leaveAuthorization => leaveAuthorization.LeaveType).WithMany(leaveType => leaveType.LeavesAuthorization)
                .HasForeignKey(leaveAuthorization => leaveAuthorization.IdLeaveType)
                .HasConstraintName("FK_LeaveAuthorization_LeaveType");
        });

        modelBuilder.Entity<Discussion>(entity =>
        {
            entity.ToTable("discussion");

            entity.HasKey(discussion => discussion.Id).HasName("PK_Discussion");
            entity.Property(discussion => discussion.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");

            entity.Property(discussion => discussion.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.HasIndex(discussion => discussion.Title, "UQ_discussion_title").IsUnique();

            entity.Property(discussion => discussion.ParticipantNumber)
                .HasColumnType("integer")
                .HasDefaultValueSql("2")
                .HasColumnName("participant_number");

            entity.Property(discussion => discussion.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });
        
        modelBuilder.Entity<UserDiscussion>(entity =>
        {
            entity.HasKey(userDiscussion => new {userDiscussion.IdUser, userDiscussion.IdDiscussion }).HasName("PK_User_Discussion");

            entity.ToTable("user_discussion");

            entity.Property(userDiscussion => userDiscussion.IdUser).HasColumnName("id_user");
            entity.Property(userDiscussion => userDiscussion.IdDiscussion).HasColumnName("id_discussion");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(userDiscussion => userDiscussion.User).WithMany(user => user.UserDiscussions)
                .HasForeignKey(userDiscussion => userDiscussion.IdUser)
                .HasConstraintName("FK_User_Discussion_User");

            entity.HasOne(userDiscussion => userDiscussion.Discussion).WithMany(discussion => discussion.UserDiscussions)
                .HasForeignKey(userDiscussion => userDiscussion.IdDiscussion)
                .HasConstraintName("FK_User_Discussion_Discussion");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("message");

            entity.HasKey(message => message.Id).HasName("PK_Message");
            entity.Property(message => message.Id)
                .HasColumnType("uuid")
                .HasColumnName("id")
                .HasDefaultValueSql("uuid_generate_v4()");

            entity.Property(message => message.IdUser).HasColumnName("id_user");
            entity.HasOne(message => message.User).WithMany()
                .HasForeignKey(message => message.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_User");

            entity.Property(message => message.IdDiscussion).HasColumnName("id_discussion");
            entity.HasOne(message => message.Discussion).WithMany( discussion => discussion.Messages)
                .HasForeignKey(message => message.IdDiscussion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_Discussion");

            entity.Property(message => message.Content)
                .HasColumnType("character varying")
                .HasColumnName("content")
                .HasMaxLength(255);

            entity.Property(message => message.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
        });

        modelBuilder.Entity<MessageState>(entity =>
        {
            entity.HasKey(messageState => new {messageState.IdMessage, messageState.IdUser }).HasName("PK_Message_State");

            entity.ToTable("message_state");

            entity.Property(messageState => messageState.IdUser).HasColumnName("id_user");
            entity.Property(messageState => messageState.IdMessage).HasColumnName("id_message");
            entity.Property(messageState => messageState.ReadAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("read_at");
            entity.Property(messageState => messageState.IsRead)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnType("boolean")
                .HasColumnName("is_read")
                .HasSentinel(false);

            entity.HasOne(messageState => messageState.User).WithMany()
                .HasForeignKey(messageState => messageState.IdUser)
                .HasConstraintName("FK_Message_State_User");

            entity.HasOne(messageState => messageState.Message).WithMany(message => message.MessageStates)
                .HasForeignKey(messageState => messageState.IdMessage)
                .HasConstraintName("FK_Message_State_Message");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
