using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class HairunSI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "discussion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    participant_number = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "2"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    size = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    public_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_6c16b9093a142e0e7613b04a3d9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "leave_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    description = table.Column<string>(type: "character varying", maxLength: 255, nullable: false),
                    is_cumulable = table.Column<bool>(type: "boolean", nullable: false),
                    solde_each_year = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "migrations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_8c82d7f526340ab734260ea46be", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_920331560282b8bd21bb02290df", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c1433d71a4838793a49dcad46ab", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id_role = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_839517a681a86bb84cbcc6a1e9d", x => x.id);
                    table.ForeignKey(
                        name: "FK_3dfe737787c5daa45d0b3b7242c",
                        column: x => x.id_role,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "permission_role",
                columns: table => new
                {
                    id_permission = table.Column<Guid>(type: "uuid", nullable: false),
                    id_role = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_be2cc366c36c67d6052b751b94f", x => new { x.id_role, x.id_permission });
                    table.ForeignKey(
                        name: "FK_4206b87fef57d4d8e3beb58efe1",
                        column: x => x.id_role,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_756cb410f074c6f61837ea4580d",
                        column: x => x.id_permission,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    id_department = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_2829ac61eff60fcec60d7274b9e", x => x.id);
                    table.ForeignKey(
                        name: "FK_ff5f0f4c0b547c6e6eb88ae4c1f",
                        column: x => x.id_department,
                        principalTable: "departments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    firstname = table.Column<string>(type: "character varying", nullable: false),
                    lastname = table.Column<string>(type: "character varying", nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    matricule = table.Column<string>(type: "character varying", nullable: false, computedColumnSql: "\nCASE\n    WHEN (length((id)::text) >= 4) THEN ('M'::text || \"substring\"(('00'::text || id), '.{LENGTH(id::text)}$'::text))\n    ELSE ('M'::text || \"substring\"(('00'::text || id), '.{3}$'::text))\nEND", stored: true),
                    id_post = table.Column<Guid>(type: "uuid", nullable: false),
                    id_file = table.Column<Guid>(type: "uuid", nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValueSql: "'example@hairun-technology.com'::character varying"),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.uuid);
                    table.ForeignKey(
                        name: "FK_1bf3c8c1ba60d51dd3e84624010",
                        column: x => x.id_post,
                        principalTable: "posts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_7b29386038a3e512a8744b8c227",
                        column: x => x.id_file,
                        principalTable: "files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leave",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    id_leave_type = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave", x => x.id);
                    table.ForeignKey(
                        name: "FK_Leave_LeaveType",
                        column: x => x.id_leave_type,
                        principalTable: "leave_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leave_User",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leave_authorization",
                columns: table => new
                {
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    id_leave_type = table.Column<Guid>(type: "uuid", nullable: false),
                    solde = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    start_validity = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    end_validity = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() + interval '1 year'"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Leave", x => new { x.id_user, x.id_leave_type });
                    table.ForeignKey(
                        name: "FK_LeaveAuthorization_LeaveType",
                        column: x => x.id_leave_type,
                        principalTable: "leave_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveAuthorization_User",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    content = table.Column<string>(type: "character varying", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    id_discussion = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.id);
                    table.ForeignKey(
                        name: "FK_Message_Discussion",
                        column: x => x.id_discussion,
                        principalTable: "discussion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Message_User",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "uuid");
                });

            migrationBuilder.CreateTable(
                name: "user_discussion",
                columns: table => new
                {
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    id_discussion = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Discussion", x => new { x.id_user, x.id_discussion });
                    table.ForeignKey(
                        name: "FK_User_Discussion_Discussion",
                        column: x => x.id_discussion,
                        principalTable: "discussion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Discussion_User",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message_state",
                columns: table => new
                {
                    id_message = table.Column<Guid>(type: "uuid", nullable: false),
                    id_user = table.Column<Guid>(type: "uuid", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    read_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message_State", x => new { x.id_message, x.id_user });
                    table.ForeignKey(
                        name: "FK_Message_State_Message",
                        column: x => x.id_message,
                        principalTable: "message",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_State_User",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_id_role",
                table: "departments",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "name_department_unique",
                table: "departments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_8681da666ad9699d568b3e91064",
                table: "departments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_discussion_title",
                table: "discussion",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_332d10755187ac3c580e21fbc02",
                table: "files",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_leave_id_leave_type",
                table: "leave",
                column: "id_leave_type");

            migrationBuilder.CreateIndex(
                name: "IX_leave_id_user",
                table: "leave",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_leave_authorization_id_leave_type",
                table: "leave_authorization",
                column: "id_leave_type");

            migrationBuilder.CreateIndex(
                name: "UQ_leave_type_designation_unique",
                table: "leave_type",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_id_discussion",
                table: "message",
                column: "id_discussion");

            migrationBuilder.CreateIndex(
                name: "IX_message_id_user",
                table: "message",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_message_state_id_user",
                table: "message_state",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_permission_role_id_permission",
                table: "permission_role",
                column: "id_permission");

            migrationBuilder.CreateIndex(
                name: "name_permission_unique",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_48ce552495d14eae9b187bb6716",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_id_department",
                table: "posts",
                column: "id_department");

            migrationBuilder.CreateIndex(
                name: "name_post_unique",
                table: "posts",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_af95ddf25e9bd491236781b1aef",
                table: "posts",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name_role_unique",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_648e3f5447f725579d7d4ffdfb7",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_discussion_id_discussion",
                table: "user_discussion",
                column: "id_discussion");

            migrationBuilder.CreateIndex(
                name: "email_user_unique",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_id_file",
                table: "users",
                column: "id_file");

            migrationBuilder.CreateIndex(
                name: "IX_users_id_post",
                table: "users",
                column: "id_post");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leave");

            migrationBuilder.DropTable(
                name: "leave_authorization");

            migrationBuilder.DropTable(
                name: "message_state");

            migrationBuilder.DropTable(
                name: "migrations");

            migrationBuilder.DropTable(
                name: "permission_role");

            migrationBuilder.DropTable(
                name: "user_discussion");

            migrationBuilder.DropTable(
                name: "leave_type");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "discussion");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
