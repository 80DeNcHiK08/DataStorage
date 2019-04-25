using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataStorage.BLL.DTOs;

namespace DataStorage.BLL.Interfaces
{
    public interface ISharingService
    {
         Task<string> OpenPublicAccess(string documentId, ClaimsPrincipal user);
         Task ClosePublicAccess(string documentId, ClaimsPrincipal user);
         Task<DocumentDTO> GetPublicDocumentByLinkAsync(string link);
         Task<string> OpenLimitedAccess(string documentId, ClaimsPrincipal user, string guestEmail);
         Task CloseLimitedAccessForUser(string documentId, ClaimsPrincipal user, string guestEmail);
        //  Task CloseLimitedAccessEntirely(string documentId, ClaimsPrincipal user);
    }
}