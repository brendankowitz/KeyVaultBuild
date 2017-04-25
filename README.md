# KeyVaultBuild [![Build status](https://ci.appveyor.com/api/projects/status/x4d8swrs91ja9m07?svg=true)](https://ci.appveyor.com/project/brendankowitz/keyvaultbuild) [![NuGet](http://img.shields.io/nuget/v/KeyVaultBuildTask.svg)](https://www.nuget.org/packages/KeyVaultBuildTask/)

## Getting started

```
Install-Package KeyVaultBuildTask
```

### Converting config files to templates

A CLI tool is provided to help convert your existing files into .keyvault.template files that the build task will process and substitute during build time.

An example of how to do this is as follows:
```
KeyVaultBuild.Cli.exe template -[configFile|c] "c:\full\path\to\App.config"
                               -[aadDirectory|d] <GUID>
                               -[vault|v] keyvaultName
                               -[appsettings|a] ^appsettingspattern$
```

### Keytemplate syntax

Below is a simple App.keyvault.template file showing keys that will be substituted at build time.

In the following key `#{keyvault:keyvaultbuild:appsetting-key1password}`, the first part is a marker `#{keyvault:` then is the name of the vault `keyvaultbuild` and following this is the name of the key that will be fetched `app-settingkey1password`.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="key1password" value="#{keyvault:keyvaultbuild:appsetting-key1password}" />
    <add key="key2secret" value="#{keyvault:keyvaultbuild:appsetting-key2secret}" />
    <add key="key3" value="test3" />
  </appSettings>
  <connectionStrings>
    <add name="hey" connectionString="#{keyvault:keyvaultbuild:connectionstring-hey}" />
  </connectionStrings>
</configuration>
```

### Using the build task to pull values from keyvault

Development:

To use interactive login, which is the preferred mode during development, you only need to specify the AAD directory (`KeyVaultBuild_DirectoryId`) which  developers can authenticate against. When a key is needed a login modal will be presented for the developer to login, if they have fetch access on the specified keyvault the key is retreived and the config file will be created.

Build server:

On the build server a service principal can be used to fetch the keys, in this case the `ClientId` and `Secret` should also be specified.

```
<KeyVaultBuild_VaultAliases Condition=" '$(KeyVaultBuild_VaultAliases)' == '' "></KeyVaultBuild_VaultAliases>
<KeyVaultBuild_ClientId Condition=" '$(KeyVaultBuild_ClientId)' == '' "></KeyVaultBuild_ClientId>
<KeyVaultBuild_Secret Condition=" '$(KeyVaultBuild_Secret)' == '' "></KeyVaultBuild_Secret>
<KeyVaultBuild_DirectoryId Condition=" '$(KeyVaultBuild_DirectoryId)' == '' "></KeyVaultBuild_DirectoryId>
```

### Advanced
Vault Aliases:
The setting VaultAlias allows your config files to reference a vault name, for example "myvault" and we can then specify an alias in the Vault Alias setting `myvault:myvault-preprod`. Doing this will force the build task to look in the `myvault-preprod` vault instead of the original that was specified. This can be handy during development and easily overridden on a build server.
