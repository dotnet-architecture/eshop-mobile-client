# eshop-mobile-client

`eshop-mobile-client` is a reference [.NET MAUI](https://dot.net/maui) multi-platform client app whose imagined purpose is to serve the mobile workforce of a fictitious company that sells products. The app allows you to manage the catalog, view products, and manage the basket and the orders.

# Dependencies

Though `eshop-mobile-client` mobile app relies on the repo [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers) for its backend but by default it uses its internal MockServices for all its functionalities. For more details refer to the [Setup](https://github.com/dotnet-architecture/eshop-mobile-client#setup) section.

<img src="media/eShopOnContainers_Architecture_Diagram.png" alt="eShopOnContainers" Width="800" />

## Architecture

The app architecture consists of two parts:

1. A .NET MAUI mobile app for iOS, macOS, Android, and Windows.
2. Several .NET Web API microservices are deployed as Docker containers.

### .NET MAUI App

This project exercises the following platforms, frameworks, and features:

- .NET MAUI
  - XAML
  - Behaviors
  - Bindings
  - Converters
  - Central Styles
  - Animations
  - IoC
  - Messaging Center
  - Custom Controls
  - xUnit Tests
- Azure Mobile Services
  - C# backend
  - WebAPI
  - Entity Framework
  - Identity Server 4

### Backend Services

All the backend services-related code and components are maintained as [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers) repo.

## Supported platforms

The app targets **four** platforms:

- iOS
- Android
- macOS (must build and deploy from Mac)
- Windows (must build and deploy from Windows)

## Requirements

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (2022 or higher) to compile C# language features
  - **Visual Studio Community Edition is fully supported!**
- .NET MAUI add-ons for Visual Studio (available via the Visual Studio installer)

or

- [Visual Studio Code](https://learn.microsoft.com/dotnet/maui/get-started/installation?tabs=visual-studio-code) configured for .NET MAUI development on Mac, Windows, or Linux. 

## Setup

### 1. Ensure the .NET MAUI platform is installed

You can do that by following the steps mentioned in [Installing .NET MAUI](https://learn.microsoft.com/dotnet/maui/get-started/installation)

### 2. Ensure .NET MAUI is updated

Visual Studio will periodically automatically check for updates. You can also manually check for updates using the [Update Visual Studio](https://docs.microsoft.com/visualstudio/install/update-visual-studio) options.

### 3. Project Setup

Restore NuGet packages for the project.

### 4. Ensure Android Emulator is installed

You can use any Android emulator or device. Refer to the [Android Emulator documentation](https://learn.microsoft.com/dotnet/maui/android/emulator/device-manager) for details on setup.

> **Note**: Android emulators cannot run well inside a virtual machine or over Remote Desktop or VNC since it relies on virtualization.

To deploy and debug the application on a physical device, refer to the [Debug on an Android device](https://learn.microsoft.com/dotnet/maui/android/device/setup) article.

### 5. Optional iOS Deployment

To deploy to iOS you can directly deploy from a Mac machine or optionally from a Windows machine [directly to a device](https://learn.microsoft.com/dotnet/maui/ios/hot-restart) or by [pairing your Windows machine to a Mac](https://learn.microsoft.com/dotnet/maui/ios/pair-to-mac).

### 6. Use Actual Microservices

By default `eshop` multiplatform client uses the internal mock services to let the user explore different sets of features in the app.

But if you want to test out the app using real services, you can do that too.

For that you'll need to do the following:

1. Deploy the backend services of `eShop` applications from the [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers) repo. You can deploy the application to either [Local Kubernetes](https://github.com/dotnet-architecture/eShopOnContainers/wiki/Deploy-to-Local-Kubernetes) or [AKS](<https://github.com/dotnet-architecture/eShopOnContainers/wiki/Deploy-to-Azure-Kubernetes-Service-(AKS)>) environments.

2. Enable microservices endpoint in the `Settings` section.

  <img src="media/microservices-endpoint-enable.png" alt="Microservices Endpoint Enable" Width="300" />

  <img src="media/microservices-endpoint-configure.png" alt="Microservices Endpoint Configure" Width="300" />

Identity Url : `http://<YOUR_IP_OR_DNS_NAME>/identity`
Mobile Gateway Shopping Url: `http://<YOUR_IP_OR_DNS_NAME>/mobileshoppingapi`

3. Enable HTTP traffic.

#### Android App

You'll also need to include the `<YOUR_IP_OR_DNS_NAME>` in the section `<domain includeSubdomains="true"><YOUR_IP_OR_DNS_NAME></domain>` of the `network_security_config.xml` file to use `HTTP` traffic.

For more details refer to [Managing HTTP & Cleartext Traffic on Android with Network Security Configuration](https://learn.microsoft.com/dotnet/maui/data-cloud/local-web-services)

#### IOS App

You'll need to make sure your `info.plist` file contains the following configuration.

```xml
  <key>NSAppTransportSecurity</key>
  <dict>
    <key>NSAllowsArbitraryLoads</key>
    <true/>
  </dict>
```

For more details refer to [Opting-Out of ATS](https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/local-web-services#ios-ats-configuration)

> **NOTE:** Please note, in production scenario you'll the services which uses `HTTPS` endpoint.

### 7. Setup Google Maps for Android

In the `eShopOnContainers\Platforms\Android\AndroidManifest.xml` you must replace the value of the `com.google.android.geo.API_KEY` with a valid Google Maps API key.

```xml
<meta-data android:name="com.google.android.geo.API_KEY" android:value="YOUR_KEY_GOES_HERE" />
```

For more details refer to [.NET MAUI Map - Get a Google Maps API key](https://learn.microsoft.com/dotnet/maui/user-interface/controls/map#get-a-google-maps-api-key).

## Screenshots

<img src="media/auth_screen.png" alt="Login" Width="210" />
<img src="media/login_screen.png" alt="Login" Width="210" />
<img src="media/catalog_screen.png" alt="Catalog" Width="210" />
<img src="media/profile_screen.png" alt="Profile" Width="210" />
<img src="media/order_screen.png" alt="Order details" Width="210" />

## Clean and Rebuild

If you see build issues when pulling updates from the repo, try cleaning and rebuilding the solution.

## Troubleshooting

## Licenses

This project uses some third-party assets with a license that requires attribution:

- [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui)
- [MVVM Community Toolkit](https://github.com/CommunityToolkit/WindowsCommunityToolkit)
- [PCLCrypto](https://github.com/AArnott/PCLCrypto)
- [IdentityModel](https://github.com/IdentityModel)

## Copyright and license

- Code and documentation copyright 2023 Microsoft Corp. Code released under the [MIT license](https://opensource.org/licenses/MIT).
