# Deploying VOCENuOrderApp to Hostinger VPS

## Prerequisites

- Hostinger VPS with **Ubuntu 22.04 + ASP.NET** template
- SSH access to the VPS
- A domain name pointed to your VPS IP

## 1. Install PostgreSQL on the VPS

```bash
sudo apt update && sudo apt install -y postgresql postgresql-contrib
sudo -u postgres psql -c "CREATE USER voce WITH PASSWORD 'YOUR_STRONG_PASSWORD' CREATEDB;"
sudo -u postgres psql -c "CREATE DATABASE voce_nuorder OWNER voce;"
```

## 2. Install .NET 8 Runtime (if not pre-installed)

```bash
sudo apt install -y dotnet-sdk-8.0
```

## 3. Publish and Upload the App

On your local machine:

```bash
cd VOCENuOrderApp
dotnet publish -c Release -o ./publish
```

Upload the `publish/` folder to the VPS:

```bash
scp -r publish/* root@YOUR_VPS_IP:/var/www/vocenuorder/
```

## 4. Apply Database Migrations

On the VPS:

```bash
cd /var/www/vocenuorder
ASPNETCORE_ENVIRONMENT=Production \
  ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=voce_nuorder;Username=voce;Password=YOUR_STRONG_PASSWORD" \
  dotnet VOCENuOrderApp.dll --migrate
```

Or run migrations from your local machine before uploading:

```bash
dotnet ef database update --connection "Host=YOUR_VPS_IP;Port=5432;Database=voce_nuorder;Username=voce;Password=YOUR_STRONG_PASSWORD"
```

## 5. Configure the systemd Service

```bash
sudo cp vocenuorder.service /etc/systemd/system/
sudo nano /etc/systemd/system/vocenuorder.service   # Fill in CHANGE_ME values
sudo systemctl daemon-reload
sudo systemctl enable vocenuorder
sudo systemctl start vocenuorder
sudo systemctl status vocenuorder
```

## 6. Configure NGINX

```bash
sudo cp nginx-vocenuorder.conf /etc/nginx/sites-available/vocenuorder
sudo ln -s /etc/nginx/sites-available/vocenuorder /etc/nginx/sites-enabled/
sudo nano /etc/nginx/sites-available/vocenuorder   # Set your domain
sudo nginx -t
sudo systemctl reload nginx
```

## 7. SSL Certificate (Let's Encrypt)

```bash
sudo apt install -y certbot python3-certbot-nginx
sudo certbot --nginx -d your-domain.com
```

## 8. Verify

- App: `https://your-domain.com`
- Health check: `https://your-domain.com/health`
- Hangfire dashboard: `https://your-domain.com/hangfire`

## Environment Variables Reference

| Variable | Description |
|---|---|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string |
| `NuOrderVOCEProduction__ConsumerKey` | NuOrder OAuth consumer key |
| `NuOrderVOCEProduction__ConsumerSecret` | NuOrder OAuth consumer secret |
| `NuOrderVOCEProduction__Token` | NuOrder OAuth token |
| `NuOrderVOCEProduction__TokenSecret` | NuOrder OAuth token secret |
| `ASPNETCORE_ENVIRONMENT` | Must be `Production` |
| `ASPNETCORE_URLS` | `http://localhost:5000` |
