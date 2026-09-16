# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2024-12-30

### Added
- Initial release
- SQL query execution with SSMS-like interface
- Database object explorer (tables, views, stored procedures, functions)
- Responsive UI with collapsible sidebar
- Authentication with SQL Server credentials
- Result limiting (1000 rows) for performance
- Query restrictions for security (blocks DROP, DELETE, etc.)
- Three connection string configuration options
- Horizontal scrolling for wide result sets
- Keyboard shortcuts (F5 to execute)
- Query formatting
- Mobile responsive design

### Security
- SQL injection prevention
- Session-based authentication
- Query timeout protection
- Dangerous command blocking

## [1.1.0] - 2026-09-16

### Added
- Query encryption before transmission to hide SQL from network inspection
- AES-256-CBC encryption on frontend using Web Crypto API
- SHA-256 key derivation for consistent 256-bit key on both client and server
- Backend decryption using SHA-256 hashed key with AES-CBC
- Anti-forgery token sent as form field for compatibility with `[ValidateAntiForgeryToken]`

### Changed
- Query parameter encrypted client-side before sending to `ExecuteQuery` endpoint
- `authenticate()` now returns `encryptionKey` to client for session storage
- `executeQuery()` encrypts query using AES-256-CBC before sending via `application/x-www-form-urlencoded`
- `DecryptQuery()` in controller derives 256-bit key via SHA-256 before decryption
- Anti-forgery token sent as `__RequestVerificationToken` form field instead of JSON body

### Security
- Query text encrypted in transit using AES-256-CBC
- SHA-256 key derivation ensures consistent encryption regardless of key string length
- Web Crypto API for client-side encryption (no external dependencies)