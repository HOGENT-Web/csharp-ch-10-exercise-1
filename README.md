# Chapter 10 - Exercise 1
##  Objectives
- Learn how to integrate authentication with the use of Auth0
- Learn how to integrate authorization with the use of Auth0
- Learn how to list all administrators programatically in your own Blazor application and not using the Auth0 dashboard.
- Learn how to create a new administrator from your own application and not using the Auth0 dashboard.

## Goal 
In this exercise we are going to extend our a e-commerce website with Authentication and Authorization. Normal customers, don't have to login. A customer can order items without being authorized, using their email we'll add them to the database. However to add, delete or to update a product an authenticated Administrator is required. 

## Exercise
### Getting Started
- Clone [the repository of Chapter 9](https://github.com/HOGENT-Web/csharp-ch-9-exercise-1/tree/solution)
- Switch to the `solution` branch
- Update to .NET6 
    - In every `.csproj` Replace all `<TargetFramework>net5.0</TargetFramework>` with `<TargetFramework>net6.0</TargetFramework>`
    - Update all NuGet packages in the entire solution

### Authentication
Implement the following using the article [Blazor Authentication with Auth0](https://benjaminvertonghen.medium.com/role-based-acces-control-with-blazor-and-auth0-i-ffd9656e6f01?sk=b8c9e562c78f620d6856e737c62927aa):
- Create a new EU based Tenant in Auth0, called `{lastname-firstname}-bogus-store`, note that another person could have already used the domain. How to create a tenant is explained in the [Auth0 Documentation - Create Tenants](https://auth0.com/docs/get-started/create-tenants)
    > A tenant is a sub division, for example a client.
- Follow the steps in the article so that all create and update actions are protected at the client and API level, only `Authenticated` users can see or actually complete the actions.
- On the right side of the Header component, show the name / email of the logged in user when he is authenticated combined with a logout button and a login button if he isn't authenticated.
- However, unauthenticated users can still see the Index and Detail pages and make API calls. In the article the default `HttpClient` is always looking for a `AccessToken`, which will throw if you don't have one (default when unauthenticated). Therefore you need a `PublicClient` that can be used by all unauthenticated calls (GetIndex and GetDetail). You can use the [this article by Chris Sainty](https://chrissainty.com/avoiding-accesstokennotavailableexception-when-using-blazor-webassembly-hosted-template-with-individual-user-accounts/) to setup the public client.  

### Authorization
Currently you can only target logged-in or logged-out users, what if we have different roles in our company and other people have less access than `Administrators`? In the next article you'll learn how to use roles and assign them, it's not as trivial as you might think...

Implement the following using the article [Blazor Authorization with Auth0](https://benjaminvertonghen.medium.com/blazor-authorization-with-auth0-rbac-d65cd14acab2?sk=1c7d500ef3c2c5e224f5040a0b03f54a):
- Create a new role in the Auth0 dashboard called `Secretary` the person with this role can only add and edit products, but not delete them.
- Create a new role in the Auth0 dashboard called `Administrator` the person with this role can do all mutating actions (Create,Delete,Edit and View).
- Create two new users in the Auth0 dashboard, how to create users is explained in the [Auth0 documentation - Create Users](https://auth0.com/docs/users/create-users).
- Assign the `Secretary` role to one of the users. How to assign roles to users is explained in the [Auth0 documentation - Assign Roles](https://auth0.com/docs/authorization/rbac/rbac-users/assign-roles-to-users).
- Assign the `Administrator` role to the other user. How to assign roles to users is explained in the [Auth0 documentation - Assign Roles](https://auth0.com/docs/authorization/rbac/rbac-users/assign-roles-to-users).
- Make sure these new roles are reflected in the client and API, so that a secretary cannot delete products, but can Edit / Add Products, the Administrator can do everything.

### Using the Management API to query
The Auth0 dashboard is pretty sweet, but it's not the best user experience in the world (leaving your app to login into Auth0 to manage users). Let's show our users in a new page called `Users` which we can navigate to from the `Header` component but only if we're logged-in as an `Administrator`.

Implement the following using the article [Blazor with Auth0, using the Management API](https://benjaminvertonghen.medium.com/blazor-with-auth0-using-the-management-api-23eda404dfef?sk=93888fb900875e01e7053ba53958445d):
- Create a new page called `Users/Index` (Users Folder with a component called `Index.razor`), which shows a simple table (nothing too fancy, but you might want to use BULMA's).
    - Show the following Table headers:
        - Firstname
        - Lastname
        - Email
- Create the following classes/interface, to eventually get all the users from Auth0 using the Management API in your own page.
    - Create a new `UserDto.Index` in the Shared Project, with the following properties:
        - Firstname
        - Lastname
        - Email
    - Create a new `UserRequest.GetIndex` in the Shared project, you can go crazy and add some search parameters if you like, searching for a person by it's name might be a good use case (optional).
    - Create a new `UserResponse.GetIndex` in the Shared project, return a list of UserDto.Index.
    - `IUserService` with a method called `GetIndexAsync` in the Shared Project 
    - Create a new `UserService` in the Services Project 
        - This service uses the `ManagementAPIClient` as explained in the article. 
    - Create a new `UserController` in the Server Project 
    - Create a new `UserService` in the Client Project 
- The Users/`Index.razor` page uses the `IUserService` which is implemented in the `Client/UserService` and calls the `UserController`, which passes the request down to the `Services/UserService` which in his term uses the Management API which as explained in the article.
    > Actually, this is quite a lot of copy-paste so get those CTRL+C's and V's ready!
    
### Using the Management API to create
We cannot only query data but also add and mutate data, let's make it possible to create a user using the management API. How to create users using the management API is explained in the [Auth0 documentation - Create a User (Management API)](https://auth0.com/docs/api/management/v2#!/Users/post_users)

- Create a new component `Users/Components/Create.razor` which uses a `UsersDto.Create` as a `EditForm` model which is validated by `FluentValidation` and has the following properties:
    - Firstname (Required)
    - Lastname (Required)
    - Email (Required - Email)
    - Password (Required - type="password"), so you cannot read it.
- Add a button to the `Users/Index.razor` page which opens a sidepanel with a `Users/Components/Create.razor` component in it. 
- In the `Users/Components/Create.razor` add a submit button which is only triggered if the validation is valid. When it's invalid it shows a `ValidationSummary` of all the errors at the top of the component. However, when it's valid it calls the `(I)UserService.CreateAsync` method, just like the one from `ProductService` with the `UsersDto.Create` as a parameter.
- Make sure to protect the button and the API call (Admins only)
- In the `Services/UserService.CreateAsync` you can use the Management API to add the user to Auth0.

Some code to get you started (this is to call the Management API, do not re-use their Requests or Responses in your client!)
```cs
var createRequest = new UserCreateRequest
{
    Email = user.Email,
    FirstName = user.FirstName,
    LastName = user.LastName,
    Password = user.Password,
    Connection = "Username-Password-Authentication",
    VerifyEmail = false,
    EmailVerified = false,
};
var createdUser = await _managementApiClient.Users.CreateAsync(createRequest);
```

### Extra
- Since the created user is in no role, he can't do anything. In the CreateAsync call, after you created the user in Auth0, use the `AssignRolesAsync` from the Auth0 SDK to add it to the `Administrator` role.
```cs
var allRoles = await _managementApiClient.Roles.GetAllAsync(new GetRolesRequest());
var adminRole = allRoles.First(x => x.Name == "Administrator");

var assignRoleRequest = new AssignRolesRequest
{
    Roles = new string[] { adminRole.Id }
};
await _managementApiClient.Users.AssignRolesAsync(createdUser?.UserId, assignRoleRequest);

```

## Solution
A possible solution can be found [here](https://github.com/HOGENT-Web/csharp-ch-10-exercise-1/tree/solution#solution).
