using AutoMapper;

namespace TDTU.API.Dtos.OrderStatusDTO;

public class OrderStatusDto : BaseStatusDto
{
	public string Name { get; set; }
	public string Description { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<OrderStatus, OrderStatusDto>();
		}
	}
}
