#/bin/bash

# Trust dotnet developer certs
dotnet dev-certs https --check --trust

# Install NX globally
npm install -g nx