# EghalTech

An end-to-end **learning project**: a sample e-commerce web app built
with **ASP.NET Core MVC**, **Entity Framework Core**, and **Stripe
Checkout**.

This project is intentionally designed to go **beyond beginner
tutorials**, applying advanced real-world patterns like **soft
deletes**, the **repository pattern**, **Fluent API configurations**,
**seeding strategies**, and **AJAX-based dynamic requests**.

## 🚀 Tech Stack

-   **ASP.NET Core MVC (.NET 8)**
-   **Entity Framework Core** (SQL Server)
-   **ASP.NET Core Identity** (custom `User`)
-   **Stripe Checkout** (server-side sessions)
-   **AJAX** (jQuery / Fetch API for dynamic requests)

## ✨ Advanced Features & Highlights

-   **Soft Delete for Users**
    -   Instead of permanently deleting users, the `IsDeleted` flag
        preserves their data (Orders, Reviews) for consistency.\
    -   Cart & Wishlist are cascaded and deleted, but Orders/Reviews
        remain intact for history.
-   **Repository Pattern**
    -   Decouples data access logic from controllers.\
    -   Generic `Repository<T>` supports reusable CRUD, while
        specialized repos add feature-specific queries.
-   **Fluent API Configurations**
    -   Enforces relationships, column types, and delete behaviors.\
    -   Example: `OnDelete: Restrict` on `OrderItems` → prevents
        accidental loss of order history.
-   **Seed Data**
    -   Categories & Products seeded via configuration classes for quick
        testing.
-   **Entity Relationships**
    -   Cart (1-1 with User) → CartItems (1-many).\
    -   Wishlist (1-1 with User) → WishlistItems (1-many, with unique
        constraint).\
    -   Orders preserve history, Products cascade delete on dependent
        Reviews, etc.
-   **AJAX for Dynamic UI**
    -   Cart count and Wishlist badge update dynamically without
        reloading the page.\
    -   Add/remove items in cart or wishlist via async requests →
        smoother user experience.
-   **Payment Integration**
    -   Stripe Checkout with secure server-side session creation.\
    -   Success/Cancel flows implemented in `PaymentController`.

## 🛠 Key Features

-   Authentication & Authorization via ASP.NET Core Identity (custom
    `User` with `Name`, `Address`, `IsDeleted`).\
-   Product catalog with Categories, filtering, and search
    (`StoreController`).\
-   Shopping Cart with quantity updates & removals.\
-   Wishlist with unique constraint to avoid duplicate items.\
-   Checkout flow that converts a Cart → Order + OrderItems.\
-   Product Reviews (1--5 stars + optional comments).\
-   Repository pattern for data access.\
-   ViewComponents for dynamic UI badges (Cart/Wishlist counts).\
-   AJAX requests for seamless cart/wishlist updates.

## 📂 Solution Layout

-   **Controllers/** -- MVC endpoints (see table below).\
-   **Models/** -- Entities (Cart, CartItem, Category, Order, Product,
    Review, User, WishList, etc.).\
-   **Repository/** -- Generic repo + helpers (e.g.,
    `UserDataCleaner`).\
-   **Configuration/** -- Fluent API configs (Category, Product, User,
    WishlistItem).\
-   **Data/** -- `AppDbContext` + `ApplyConfigurationsFromAssembly`.\
-   **Views/** -- Razor pages for each feature.

## 📊 Controllers & Actions

  -------------------------------------------------------------------------------
  Controller           Actions
  -------------------- ----------------------------------------------------------
  AccountController    ChangePassword, DeleteAccount, Edit, LogIn, Logout,
                       Profile, ReActivateDeletedUser, Register

  CartController       AddToCart, Delete, Index, UpdateQuantity

  CategoryController   Add, Delete, Edit, Index, SearchCategories

  HomeController       Error, Index, Privacy

  OrderController      Cancel, Delete, Details, Index, UpdateStatus

  PaymentController    Cancel, CreateCheckoutSession, Success

  ProductController    AddProduct, Delete, Edit, Index, SearchProducts,
                       ValidatePriceInput, ValidateStockQtyInput

  ReviewController     Add, Delete, Index

  StoreController      Index, ProductDetails, Search

  WishListController   Delete, HasItems, Index, ToggleWishList
  -------------------------------------------------------------------------------

## 💳 Payments

Stripe Checkout handles payments securely:

``` bash
dotnet user-secrets init
dotnet user-secrets set "Stripe:SecretKey" "<sk_test_...>"
dotnet user-secrets set "Stripe:PublishableKey" "<pk_test_...>"
```

## 🗄 Database & Seeding

-   `CategoryConfiguration` & `ProductConfiguration` seed demo data.\
-   Relationships, column types, and delete behaviors enforced with
    Fluent API.\

``` bash
dotnet ef database update
```

## ▶️ How to Run

1.  Install **.NET 8 SDK** + **SQL Server** (or SQL Server Express).\

2.  Update the connection string in `appsettings.Development.json`.\

3.  Configure Stripe keys via `dotnet user-secrets`.\

4.  Run:

    ``` bash
    dotnet restore
    dotnet ef database update
    dotnet run
    ```

## 📝 Domain Rules

-   Wishlist enforces uniqueness (no duplicate products).\
-   Product deletion restricted if referenced in an Order.\
-   Orders & Reviews are preserved even if the User is soft-deleted.\
-   Cart/Wishlist cascade with User deletion.

------------------------------------------------------------------------

⚡ **Note:** This project is for **learning purposes** --- to practice
real-world ASP.NET Core concepts like EF Core configurations, repository
pattern, AJAX requests, and payment integration.
