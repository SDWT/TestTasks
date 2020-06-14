FROM mcr.microsoft.com/dotnet/core/sdk

LABEL author="Dan Wahlin"

ENV ASPNETCORE_URLS=http://+:5000

WORKDIR /var/www/Extra

COPY ./Extra .

WORKDIR /var/www/koshelektesttask

COPY ./Koshelek.TestTask .

EXPOSE 5000

ENTRYPOINT ["/bin/bash", "-c", "dotnet restore && dotnet run"]
