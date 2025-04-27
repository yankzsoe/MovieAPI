using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Application.DTOs.Auth {
    public class AuthResponse {
        public string TokenType { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public UserDto User { get; private set; }

        public AuthResponse(string tokenType, string token, DateTime expiresAt, UserDto user) {
            TokenType = tokenType;
            Token = token;
            ExpiresAt = expiresAt;
            User = user;
        }
    }
}
