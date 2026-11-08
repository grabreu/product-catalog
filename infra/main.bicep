param location string = 'brazilsouth'

@description('Image reference, tag or digest. Required on every deploy so a redeploy does not revert the image published by CD.')
param image string

@description('Name of the managed certificate bound to the custom domain. Leave empty until the hostname is bound and the certificate is created through the CLI; a deploy with it empty removes the custom domain.')
param customDomainCertificateName string = ''

var tags = {
  project: 'productcatalog'
  env: 'prod'
  owner: 'grabreu'
}

module db 'modules/sql-database.bicep' = {
  name: 'db-deploy'
  params: {
    location: location
    tags: tags
  }
}

module api 'modules/container-app.bicep' = {
  name: 'api-deploy'
  params: {
    location: location
    tags: tags
    image: image
    sqlServerFqdn: db.outputs.sqlServerFqdn
    sqlDatabaseName: db.outputs.sqlDatabaseName
    customDomainCertificateName: customDomainCertificateName
  }
}
