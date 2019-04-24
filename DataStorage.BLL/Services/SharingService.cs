using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DataStorage.BLL.DTOs;
using DataStorage.BLL.Interfaces;
using DataStorage.DAL.Entities;
using DataStorage.DAL.Interfaces;
using DataStorage.DAL.Repositories;

namespace DataStorage.BLL.Services
{
    public class SharingService : ISharingService
    {
        private readonly IDocumentRepository _documentRepo;
        private readonly IUserDocumentRepository _userDocumentRepo;
        private readonly IUsersRepository _userRepo;
        private readonly IMapper _mapper;

        public SharingService(IDocumentRepository documentRepo, IUserDocumentRepository userDocRepo, IUsersRepository userRepo, IMapper mapper)
        {
            _documentRepo = documentRepo ?? throw new ArgumentNullException(nameof(documentRepo));
            _userDocumentRepo = userDocRepo ?? throw new ArgumentNullException(nameof(userDocRepo));
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> OpenPublicAccess(string documentId, ClaimsPrincipal user)
        {
            string ownerId = _userRepo.GetUserId(user);

            if (!(await _documentRepo.IsUserDocumentOwner(documentId, ownerId)))
            {
                throw new UnauthorizedAccessException($"The user {ownerId} is not the owner of the document");
            }

            var document = await _documentRepo.GetDocumentByIdAsync(documentId);
            if (!document.IsPublic)
            {
                document.IsPublic = true;
                document.DocumentLink = _documentRepo.GenerateAccessLink();
                await _documentRepo.UpdateDocumentAsync(document);
                return document.DocumentLink;
            }

            return document.DocumentLink;
        }

        public async Task ClosePublicAccess(string documentId, ClaimsPrincipal user)
        {
            string ownerId = _userRepo.GetUserId(user);

            if (!(await _documentRepo.IsUserDocumentOwner(documentId, ownerId)))
            {
                throw new UnauthorizedAccessException($"The user {ownerId} is not the owner of the document");
            }

            var document = await _documentRepo.GetDocumentByIdAsync(documentId);
            document.IsPublic = false;
            document.DocumentLink = string.Empty;
            await _documentRepo.UpdateDocumentAsync(document);
        }

        public async Task<DocumentDTO> GetPublicDocumentByLinkAsync(string link)
        {
            var document = await _documentRepo.GetPublicDocumentByLinkAsync(link);
            return _mapper.Map<DocumentDTO>(document);
        }

        public async Task<string> OpenLimitedAccess(string documentId, ClaimsPrincipal owner, string guestEmail)
        {
            string ownerId = _userRepo.GetUserId(owner);

            if (!(await _documentRepo.IsUserDocumentOwner(documentId, ownerId)))
            {
                throw new UnauthorizedAccessException($"The user {ownerId} is not the owner of the document");
            }

            var document = await _documentRepo.GetDocumentByIdAsync(documentId);
            if (!document.IsPublic)
            {
                var userDocumentLink = _documentRepo.GenerateAccessLink();
                var user = await _userRepo.GetUserByNameAsync(guestEmail);
                await _userDocumentRepo.CreateAsync(new UserDocument
                {
                    Document = document,
                    User = user,
                    DocumentLink = userDocumentLink
                });

                return userDocumentLink;
            }

            return await _userDocumentRepo.GetUserDocumentLinkByIdAsync(documentId);
        }

        public async Task CloseLimitedAccessForUser(string documentId, ClaimsPrincipal user, string guestEmail)
        {
            string ownerId = _userRepo.GetUserId(user);

            if (!(await _documentRepo.IsUserDocumentOwner(documentId, ownerId)))
            {
                throw new UnauthorizedAccessException($"The user {ownerId} is not the owner of the document");
            }

            var userDocument = await _userDocumentRepo.GetUserDocumentAsync(guestEmail, documentId);
            if (userDocument != null)
            {
                await _userDocumentRepo.DeleteAsync(userDocument);
            }
        }

        public async Task CloseLimitedAccessEntirely(string documentId, ClaimsPrincipal user)
        {
            string ownerId = _userRepo.GetUserId(user);

            if (!(await _documentRepo.IsUserDocumentOwner(documentId, ownerId)))
            {
                throw new UnauthorizedAccessException($"The user {ownerId} is not the owner of the document");
            }

            var userDocument = await _userDocumentRepo.GetUserDocumentByIdAsync(documentId);
            if (userDocument != null)
            {
                await _userDocumentRepo.DeleteAsync(userDocument);
            }
        }
    }
}