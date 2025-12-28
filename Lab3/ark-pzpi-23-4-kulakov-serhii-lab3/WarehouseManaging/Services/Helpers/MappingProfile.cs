using AutoMapper;
using Domain.DateTrensferObjects;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTicketDto, SupportTicket>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TicketStatus, opt => opt.Ignore())
                .ForMember(dest => dest.AnsweredAt, opt => opt.Ignore())
                .ForMember(dest => dest.ClosedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedTo, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore());
            CreateMap<SupportTicket, TicketDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TicketStatus))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src =>
                src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src =>
                src.AssignedTo != null ? src.AssignedTo.Name : null))
                .ForMember(dest => dest.AssignedToEmail, opt => opt.MapFrom(src =>
                src.AssignedTo != null ? src.AssignedTo.Email : null))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src =>
                src.AnsweredAt));
            CreateMap<SupportTicket, TicketDetailsDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TicketStatus))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
                src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src =>
                src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src =>
                src.AssignedTo != null ? src.AssignedTo.Name : null))
                .ForMember(dest => dest.AssignedToEmail, opt => opt.MapFrom(src =>
                src.AssignedTo != null ? src.AssignedTo.Email : null))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src =>
                src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.TicketTitle, opt => opt.MapFrom(src =>
                src.Ticket != null ? src.Ticket.Title : null));
            CreateMap<Attachment, AttachmentDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => $"/api/attachments/{src.Id}"));
            CreateMap<User, ProfileDto>()
                .ForMember(dest => dest.Id, opt  => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber : null))
                .ForMember(dest => dest.SecondPhoneNumber, opt => opt.MapFrom(src => src.SecondPhoneNumber != null ? src.SecondPhoneNumber : null))
                .ForMember(dest => dest.UserAdverts, opt => opt.MapFrom(src => src.UserAdverts != null ? src.UserAdverts: null));
            CreateMap<Advert, AdvertDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User));
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dest => dest.PricePerMonth, opt => opt.MapFrom(src => src.PricePerMonth))
                .ForMember(dest => dest.Scale, opt => opt.MapFrom(src => src.Scale))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor))
                .ForMember(dest => dest.BuildingType, opt => opt.MapFrom(src => src.BuildingType))
                .ForMember(dest => dest.Communications, opt => opt.MapFrom(src => src.Communications))
                .ForMember(dest => dest.HouseholdAppliances, opt => opt.MapFrom(src => src.HouseholdAppliances))
                .ForMember(dest => dest.Infrastructures, opt => opt.MapFrom(src => src.Infrastructures))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<User, AdvertAuthorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
