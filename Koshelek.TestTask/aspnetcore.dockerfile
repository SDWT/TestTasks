FROM mcr.microsoft.com/dotnet/core/sdk

LABEL author="Dan Wahlin"

ENV ASPNETCORE_URLS=http://+:5000

WORKDIR /var/Services

COPY ./Services .

WORKDIR /var/Common

COPY ./Common .

WORKDIR /var/www/koshelektesttask

COPY ./WebApp/Koshelek.TestTask .

EXPOSE 5000

ENTRYPOINT ["/bin/bash", "-c", "dotnet restore && dotnet run"]
