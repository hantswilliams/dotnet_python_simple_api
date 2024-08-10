# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project files
COPY . ./

# Install Python 3.11 and required build tools
RUN apt-get update && apt-get install -y python3.11 python3.11-venv python3-pip build-essential \
    libatlas-base-dev gfortran

# Create a virtual environment and install Python dependencies
RUN python3.11 -m venv /app/venv \
    && /app/venv/bin/pip install --upgrade pip \
    && /app/venv/bin/pip install -r requirements.txt

# Publish the .NET application
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install Python 3.11 and required runtime libraries for numpy
RUN apt-get update && apt-get install -y python3.11 python3-pip libatlas-base-dev

# Copy the published application and Python scripts from the build environment
COPY --from=build-env /app/out .
COPY hello.py .
COPY hello_name_age.py .
COPY --from=build-env /app/venv /app/venv

# Set environment variable to use the virtual environment's Python
ENV PATH="/app/venv/bin:$PATH"

# Expose the port the application runs on
EXPOSE 5005

# Run the application
ENTRYPOINT ["dotnet", "simple_api.dll"]
