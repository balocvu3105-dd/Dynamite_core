using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEconomy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaughtFish");

            migrationBuilder.DropTable(
                name: "FishEncyclopedia");

            migrationBuilder.DropTable(
                name: "FishingActivityLogs");

            migrationBuilder.DropTable(
                name: "FishingDataSnapshots");

            migrationBuilder.DropTable(
                name: "GuildLevelRoles");

            migrationBuilder.DropTable(
                name: "GuildPearlLogs");

            migrationBuilder.DropTable(
                name: "GuildPonds");

            migrationBuilder.DropTable(
                name: "LeaderboardEntries");

            migrationBuilder.DropTable(
                name: "SpecialPools");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserFishingAchievements");

            migrationBuilder.DropTable(
                name: "UserFishTrophies");

            migrationBuilder.DropTable(
                name: "UserInventories");

            migrationBuilder.DropTable(
                name: "UserServerProfiles");

            migrationBuilder.DropTable(
                name: "WeeklyActivities");

            migrationBuilder.DropTable(
                name: "UserFishBags");

            migrationBuilder.DropTable(
                name: "LeaderboardSnapshots");

            migrationBuilder.DropTable(
                name: "UserFishingProfiles");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "UserWallets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishEncyclopedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BestCoins = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false),
                    FirstCaughtAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FishName = table.Column<string>(type: "text", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LastCaughtAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Rarity = table.Column<string>(type: "text", nullable: false),
                    TimesCaught = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishEncyclopedia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishingActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoinsEarned = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Event = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FishName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PondRemaining = table.Column<int>(type: "integer", nullable: false),
                    PoolName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    RodName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Weather = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    XpEarned = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishingActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishingDataSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AchievementIds = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    BagCapacity = table.Column<int>(type: "integer", nullable: false),
                    BagSnapshotJson = table.Column<string>(type: "text", nullable: false),
                    ChestsOpened = table.Column<int>(type: "integer", nullable: false),
                    CommonCaught = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FishingLevel = table.Column<int>(type: "integer", nullable: false),
                    FishingXp = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LegendaryCaught = table.Column<int>(type: "integer", nullable: false),
                    MythicCaught = table.Column<int>(type: "integer", nullable: false),
                    RareCaught = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TotalCaught = table.Column<int>(type: "integer", nullable: false),
                    UncommonCaught = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    WalletCoins = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishingDataSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildLevelRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LevelType = table.Column<string>(type: "text", nullable: false),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildLevelRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildPearlLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PearlType = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildPearlLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildPonds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentFish = table.Column<int>(type: "integer", nullable: false),
                    CurrentWeather = table.Column<string>(type: "text", nullable: false),
                    DailyChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    DepletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FishEscapeRateOverride = table.Column<double>(type: "double precision", nullable: true),
                    FishMissRateOverride = table.Column<double>(type: "double precision", nullable: true),
                    FishingChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MaxFish = table.Column<int>(type: "integer", nullable: false),
                    ResetAvailableAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WeatherExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildPonds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CooldownSeconds = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DropMultiplier = table.Column<double>(type: "double precision", nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    Emoji = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    EscapeRate = table.Column<double>(type: "double precision", nullable: true),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    LuckBonus = table.Column<int>(type: "integer", nullable: true),
                    MaxDurability = table.Column<int>(type: "integer", nullable: true),
                    MissRate = table.Column<double>(type: "double precision", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsageCount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WeekStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialPools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DropTable = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MinLevel = table.Column<int>(type: "integer", nullable: false),
                    PoolName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RemainingFish = table.Column<int>(type: "integer", nullable: false),
                    StartsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialPools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFishBags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BagCapacity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFishBags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFishingProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AutoFishCastsToday = table.Column<int>(type: "integer", nullable: false),
                    AutoFishChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AutoFishDailyResetAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AutoFishExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AutoFishPaused = table.Column<bool>(type: "boolean", nullable: false),
                    AutoFishPurchaseCount = table.Column<int>(type: "integer", nullable: false),
                    AutoFishSellAll = table.Column<bool>(type: "boolean", nullable: false),
                    AutoFishSpecialPoolExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AutoFishSpecialPoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    AutoFishUseBait = table.Column<bool>(type: "boolean", nullable: false),
                    ChestsOpened = table.Column<int>(type: "integer", nullable: false),
                    CommonCaught = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FishingLevel = table.Column<int>(type: "integer", nullable: false),
                    FishingXp = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LastFishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LegendaryCaught = table.Column<int>(type: "integer", nullable: false),
                    MythicCaught = table.Column<int>(type: "integer", nullable: false),
                    RareCaught = table.Column<int>(type: "integer", nullable: false),
                    TotalCaught = table.Column<int>(type: "integer", nullable: false),
                    TradeWeekResetAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TradesThisWeek = table.Column<int>(type: "integer", nullable: false),
                    UncommonCaught = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFishingProfiles", x => x.Id);
                    table.UniqueConstraint("AK_UserFishingProfiles_GuildId_UserId", x => new { x.GuildId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "UserFishTrophies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FishName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IsPearl = table.Column<bool>(type: "boolean", nullable: false),
                    IsSpecial = table.Column<bool>(type: "boolean", nullable: false),
                    Rarity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFishTrophies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserServerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LastMessageXpAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServerLevel = table.Column<int>(type: "integer", nullable: false),
                    ServerXp = table.Column<long>(type: "bigint", nullable: false),
                    TotalVoiceMinutes = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    VoiceJoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServerProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Coins = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DailyStreak = table.Column<int>(type: "integer", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    LastDaily = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    WeekResetAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WeeklyFishCaught = table.Column<int>(type: "integer", nullable: false),
                    WeeklyMessages = table.Column<int>(type: "integer", nullable: false),
                    WeeklyVoiceMinutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeltaRank = table.Column<int>(type: "integer", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_LeaderboardSnapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "LeaderboardSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaughtFish",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BagId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoinValue = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FishEmoji = table.Column<string>(type: "text", nullable: false),
                    FishName = table.Column<string>(type: "text", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IsPearl = table.Column<bool>(type: "boolean", nullable: false),
                    IsSpecialCreature = table.Column<bool>(type: "boolean", nullable: false),
                    Rarity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SourcePool = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaughtFish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaughtFish_UserFishBags_BagId",
                        column: x => x.BagId,
                        principalTable: "UserFishBags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFishingAchievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AchievementId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFishingAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFishingAchievements_UserFishingProfiles_GuildId_UserId",
                        columns: x => new { x.GuildId, x.UserId },
                        principalTable: "UserFishingProfiles",
                        principalColumns: new[] { "GuildId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToWalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_UserWallets_FromWalletId",
                        column: x => x.FromWalletId,
                        principalTable: "UserWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transactions_UserWallets_ToWalletId",
                        column: x => x.ToWalletId,
                        principalTable: "UserWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcquiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RodDurability = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInventories_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInventories_UserWallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "UserWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaughtFish_BagId",
                table: "CaughtFish",
                column: "BagId");

            migrationBuilder.CreateIndex(
                name: "IX_CaughtFish_GuildId_UserId",
                table: "CaughtFish",
                columns: new[] { "GuildId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_FishingActivityLogs_CreatedAt",
                table: "FishingActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FishingActivityLogs_GuildId_CreatedAt",
                table: "FishingActivityLogs",
                columns: new[] { "GuildId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FishingActivityLogs_GuildId_UserId_CreatedAt",
                table: "FishingActivityLogs",
                columns: new[] { "GuildId", "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_FishingDataSnapshots_GuildId_UserId_CreatedAt",
                table: "FishingDataSnapshots",
                columns: new[] { "GuildId", "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GuildLevelRoles_GuildId_LevelType_RequiredLevel",
                table: "GuildLevelRoles",
                columns: new[] { "GuildId", "LevelType", "RequiredLevel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildPearlLogs_GuildId_PearlType_CreatedAt",
                table: "GuildPearlLogs",
                columns: new[] { "GuildId", "PearlType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GuildPonds_GuildId",
                table: "GuildPonds",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_GuildId_Name",
                table: "InventoryItems",
                columns: new[] { "GuildId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_SnapshotId_Rank",
                table: "LeaderboardEntries",
                columns: new[] { "SnapshotId", "Rank" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardSnapshots_GuildId_Type_WeekStartDate",
                table: "LeaderboardSnapshots",
                columns: new[] { "GuildId", "Type", "WeekStartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialPools_GuildId_StartsAt_ExpiresAt",
                table: "SpecialPools",
                columns: new[] { "GuildId", "StartsAt", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromWalletId",
                table: "Transactions",
                column: "FromWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GuildId_CreatedAt",
                table: "Transactions",
                columns: new[] { "GuildId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToWalletId",
                table: "Transactions",
                column: "ToWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFishBags_GuildId_UserId",
                table: "UserFishBags",
                columns: new[] { "GuildId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFishingAchievements_GuildId_UserId_AchievementId",
                table: "UserFishingAchievements",
                columns: new[] { "GuildId", "UserId", "AchievementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFishingProfiles_GuildId_UserId",
                table: "UserFishingProfiles",
                columns: new[] { "GuildId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFishTrophies_GuildId_UserId",
                table: "UserFishTrophies",
                columns: new[] { "GuildId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFishTrophies_GuildId_UserId_FishName",
                table: "UserFishTrophies",
                columns: new[] { "GuildId", "UserId", "FishName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_ItemId",
                table: "UserInventories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_WalletId_ItemId",
                table: "UserInventories",
                columns: new[] { "WalletId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserServerProfiles_GuildId_UserId",
                table: "UserServerProfiles",
                columns: new[] { "GuildId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWallets_GuildId_Coins",
                table: "UserWallets",
                columns: new[] { "GuildId", "Coins" });

            migrationBuilder.CreateIndex(
                name: "IX_UserWallets_GuildId_UserId",
                table: "UserWallets",
                columns: new[] { "GuildId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyActivities_GuildId_UserId",
                table: "WeeklyActivities",
                columns: new[] { "GuildId", "UserId" },
                unique: true);
        }
    }
}
